using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    /// <summary>
    /// Reprezentuje miarę wybraną przez użytkownika.
    /// </summary>
    public class SelectedMeasure : SelectedItem
    {
        /// <summary>
        /// Inicjalizuje instancję klasy SelectedMeasure za pomocą przyjaznej nazwy, wartości MDX, ścieżki do węzła MeasureTreeView.
        /// </summary>
        /// <param name="name">Przyjazna nazwa miary.</param>
        /// <param name="value">Wartość miary w języku MDX.</param>
        /// <param name="path">Ścieżka do węzła kontrolki typu MeasureTreeView, który reprezentuje miarę.</param>
        public SelectedMeasure(string name, string value, string path) : base(name, value, path) { }
    }
}