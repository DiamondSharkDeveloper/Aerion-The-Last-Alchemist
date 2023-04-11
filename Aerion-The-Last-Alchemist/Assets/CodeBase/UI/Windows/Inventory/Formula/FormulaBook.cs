using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public class FormulaBook : Book
    {
        [SerializeField] private FormulaPage pagePrefab;
        [SerializeField] private FormulasPage formulaspagePrefab;
        [SerializeField] private Sprite _rightPageSprite;
        [SerializeField] private Sprite _leftPageSprite;
        [SerializeField] private GameObject _rightPageBack;
        [SerializeField] private GameObject _leftPageBAck;
        private readonly List<MyPage> _pages = new List<MyPage>();
        private readonly List<MyPage> _clonePages = new List<MyPage>();
        private readonly List<MyPage> _backclonePages = new List<MyPage>();
        private bool _canFlip = false;

        public void Initialize(List<FormulaStaticData> formulas, IPersistentProgressService persistentProgressService,
            Action<FormulaStaticData> action)
        {
            bookPages = new List<Sprite>();
            AddPageBackGround(formulas.Count);
            _pages.Add(CreateFormulasPage(formulas, RightNext.gameObject.transform));
            for (int i = 0; i < formulas.Count; i++)
            {
                _pages.Add(CreateFormulaPage(formulas[i], CalculateParent(i + 1), false,
                    persistentProgressService.Progress.gameData.lootData, action));

                int count = i;
                _pages[count + 1].Hide();
            }

            OnPageDraging += ClosePages;
        }

        private void OnDestroy()
        {
            OnPageDraging -= ClosePages;
        }

        private Transform CalculateParent(int i)
        {
            return (i % 2) == 0 ? RightNext.gameObject.transform : LeftNext.gameObject.transform;
        }

        void Update()
        {
            if (pageDragging && interactable)
            {
                canShowNewPage = false;
                UpdateBook();
                ShowBackPage();
            }
            else
            {
                _canFlip = true;
                if (canShowNewPage)
                {
                   
                    if (_pages?.Count != 0)
                    {
                        
                        if (currentPage >= _pages?.Count && !_pages[^1].isActiveAndEnabled)
                        {
                            ShowLastPage();
                        }
                        else
                        {
                            if (_pages != null && !(currentPage >= _pages?.Count) &&
                                !_pages[currentPage].isActiveAndEnabled)
                            {
                                ShowCurrentPages();
                            }
                        }
                    }
                }
            }
        }


        private void ShowCurrentPages()
        {
            ClosePages();
            ClearBackClonePages();
            _pages[currentPage].Show();
            if (currentPage > 0)
            {
                _pages[currentPage - 1].Show();
            }
        }

        private void ClearClonePages()
        {
            for (var i = 0; i < _clonePages.Count; i++)
            {
                Destroy(_clonePages[i].gameObject);
            }

            _clonePages.Clear();
        }

        private void ClearBackClonePages()
        {
            for (var i = 0; i < _backclonePages.Count; i++)
            {
                Destroy(_backclonePages[i].gameObject);
            }

            _backclonePages.Clear();
        }

        private void ShowBackPage()
        {
            if (_canFlip)
            {
                ClearClonePages();
                switch (mode)
                {
                    case FlipMode.LeftToRight:
                    {
                        MyPage page = Instantiate(_pages[currentPage - 2], Left.transform);
                        page.gameObject.SetActive(true);
                        page.MakePageFace();

                        _clonePages.Add(page);

                        MyPage secondPage = Instantiate(_pages[currentPage - 1], Right.transform);
                        secondPage.gameObject.SetActive(true);
                        secondPage.MakePageFace();
                        _clonePages.Add(secondPage);
                        if (currentPage - 3 >= 0)
                        {
                            MyPage leftBackPage = Instantiate(_pages[currentPage - 3], _leftPageBAck.transform);
                            leftBackPage.gameObject.SetActive(true);
                            leftBackPage.MakePageFace();
                            _backclonePages.Add(leftBackPage);
                        }


                        if (_pages.Count > currentPage)
                        {
                            MyPage rightBackPage = Instantiate(_pages[currentPage], _rightPageBack.transform);
                            rightBackPage.gameObject.SetActive(true);
                            rightBackPage.MakePageFace();
                            _backclonePages.Add(rightBackPage);
                        }
                    }

                        break;
                    case FlipMode.RightToLeft:
                    {
                        MyPage page = Instantiate(_pages[currentPage + 1], Right.transform);
                        page.gameObject.SetActive(true);
                        page.MakePageFace();
                        _clonePages.Add(page);
                        MyPage secondPage = Instantiate(_pages[currentPage], Left.transform);
                        secondPage.gameObject.SetActive(true);
                        secondPage.MakePageFace();
                        _clonePages.Add(secondPage);

                        if (currentPage - 1 >= 0)
                        {
                            MyPage leftBackPage = Instantiate(_pages[currentPage - 1], _leftPageBAck.transform);
                            leftBackPage.gameObject.SetActive(true);
                            leftBackPage.MakePageFace();
                            _backclonePages.Add(leftBackPage);
                        }


                        if (_pages.Count > currentPage + 2)
                        {
                            MyPage rightBackPage = Instantiate(_pages[currentPage + 2], _rightPageBack.transform);
                            rightBackPage.gameObject.SetActive(true);
                            rightBackPage.MakePageFace();
                            _backclonePages.Add(rightBackPage);
                        }
                    }

                        break;
                }

                _canFlip = false;
            }
        }

        private void ShowLastPage()
        {
            ClosePages();
         
            _pages[^1].Show();
            ClearBackClonePages();
        }

        private void ClosePages()
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                _pages[i].Hide();
            }
        }

        FormulaPage CreateFormulaPage(FormulaStaticData formulaStaticData, Transform parent, bool isEmpty,
            LootData lootData, Action<FormulaStaticData> action)
        {
            FormulaPage page = Instantiate(pagePrefab, parent, false);
            page.SetPage(formulaStaticData, lootData, action);
            return page;
        }

        FormulasPage CreateFormulasPage(List<FormulaStaticData> formulaStaticDats, Transform parent)
        {
            FormulasPage page = Instantiate(formulaspagePrefab, parent, false);
            page.SetPage(formulaStaticDats);
            return page;
        }

        private void AddPageBackGround(int count)
        {
            for (int i = 0; i < count; i++)
            {
                bookPages.Add((i % 2) != 0 ? _leftPageSprite : _rightPageSprite);
            }
        }
    }
}