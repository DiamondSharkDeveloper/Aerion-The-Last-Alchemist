using System;
using System.Collections.Generic;
using CodeBase.StaticData;
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
            _pages.Add(CreateFormulasPage(formulas, LeftNext.gameObject.transform));
            bookPages.Add(pageSprite);
            for (int i = 0; i < formulas.Count; i++)
            {
                _pages.Add(CreateFormulaPage(formulas[i], CalculateParent(i)));
                bookPages.Add(pageSprite);
                if (i>0)
                {
                    _pages[i+1].Hide();
                }
            }

            //   OnPageDraging += ClosePages;
            OnPageChange += ShowCurrentPages;
        }

        private Transform CalculateParent(int i)
        {
            return (i > 0)
                ? ((i % 2) == 0) ? RightNext.gameObject.transform : LeftNext.gameObject.transform
                : RightNext.gameObject.transform;
        }

        private void OnDestroy()
        {
            OnPageChange -= ShowCurrentPages;
        }

        private void ShowCurrentPages()
        {
            ClosePages();
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

        FormulaPage CreateFormulaPage(FormulaStaticData formulaStaticData, Transform parent)
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
    }
}