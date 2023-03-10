using System;
using System.Collections.Generic;
using CodeBase.StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public class FormulaBook : Book
    {
        [SerializeField] private FormulaPage pagePrefab;
        [SerializeField] private FormulasPage formulaspagePrefab;
        [SerializeField] private Sprite pageSprite;
        private readonly List<MyPage> _pages = new List<MyPage>();

        public void Initialize(List<FormulaStaticData> formulas)
        {
            bookPages = new List<Sprite>();
            AddPageBackGround(formulas.Count + 1);
            _pages.Add(CreateFormulasPage(formulas, RightNext.gameObject.transform));
            for (int i = 0; i < formulas.Count; i++)
            {
                _pages.Add(CreateFormulaPage(formulas[i], CalculateParent(i + 1), false));

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
                UpdateBook();
            }
            else
            {
                if (_pages?.Count != 0)
                {
                    if (currentPage >= _pages?.Count&&!_pages[^1].isActiveAndEnabled)
                    {
                        ShowLastPage();
                    }
                    else
                    {
                        if (!_pages[currentPage].isActiveAndEnabled)
                        {
                            ShowCurrentPages();
                        }
                    }
                }
            }
        }


        private void ShowCurrentPages()
        {
            ClosePages();

            _pages[currentPage].Show();
            if (currentPage > 0)
            {
                _pages[currentPage - 1].Show();
            }
        }

        private void ShowLastPage()
        {
            ClosePages();
            _pages[^1].Show();
        }

        private void ClosePages()
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                _pages[i].Hide();
            }
        }

        FormulaPage CreateFormulaPage(FormulaStaticData formulaStaticData, Transform parent, bool isEmpty)
        {
            FormulaPage page = Instantiate(pagePrefab, parent, false);
            page.SetPage(formulaStaticData);
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
                bookPages.Add(pageSprite);
            }
        }
    }
}