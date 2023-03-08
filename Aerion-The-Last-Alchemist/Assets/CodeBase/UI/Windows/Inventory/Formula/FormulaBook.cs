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
        [SerializeField] private GameObject pageHolder;
        private readonly List<MyPage> _pages = new List<MyPage>();
        private List<FormulaStaticData> _formulas = new List<FormulaStaticData>();
        private readonly List<Action> _needToClose = new List<Action>();

        public void Initialize(List<FormulaStaticData> formulas)
        {
            bookPages = new List<Sprite>();
            _pages.Add(CreateFormulasPage(formulas, RightNext.gameObject.transform));
            _needToClose.Add(() => { _pages[0].Hide(); });
            for (int i = 0; i < formulas.Count; i++)
            {
                _pages.Add(CreateFormulaPage(formulas[i], CalculateParent(i + 1), false));

                int count = i;
                _pages[count + 1].Hide();
            }

            if ((bookPages.Count % 2) != 0)
            {
                bookPages.Add(pageSprite);
            }
            OnPageDraging += ClosePages;
            OnPageChange += ShowCurrentPages;
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
                ClosePages();
            }
            else
            {
                if (!_pages[currentPage].isActiveAndEnabled)
                {
                    ShowCurrentPages();
                }
            }
        }

        private void OnDestroy()
        {
            OnPageChange -= ShowCurrentPages;
            OnPageDraging += ClosePages;
        }

        private void ShowCurrentPages()
        {
            _pages[currentPage].Show();
            _needToClose.Add(() => { _pages[currentPage].Hide(); });
            if (currentPage > 0)
            {
                _pages[currentPage - 1].Show();
                _needToClose.Add(() => { _pages[currentPage - 1].Hide(); });
            }
        }

        private void ClosePages()
        {
            if (_needToClose?.Count > 0)
            {
                foreach (Action action in _needToClose)
                {
                    action.Invoke();
                }

                _needToClose.Clear();
            }
        }

        FormulaPage CreateFormulaPage(FormulaStaticData formulaStaticData, Transform parent, bool isEmpty)
        {
            FormulaPage page = Instantiate(pagePrefab, parent, false);

            page.SetPage(formulaStaticData);

            bookPages.Add(pageSprite);
            return page;
        }

        FormulasPage CreateFormulasPage(List<FormulaStaticData> formulaStaticDats, Transform parent)
        {
            FormulasPage page = Instantiate(formulaspagePrefab, parent, false);
            page.SetPage(formulaStaticDats);
            bookPages.Add(pageSprite);
            return page;
        }
    }
}