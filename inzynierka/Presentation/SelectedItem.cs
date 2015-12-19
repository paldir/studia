using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    /// <summary>
    /// Reprezentuje poziom wymiaru lub miarę wybraną przez użytkownika.
    /// </summary>
    public class SelectedItem
    {
        /// <summary>
        /// Przyjazna nazwa.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Wartość w języku MDX.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Ścieżka do węzła kontrolki typu TreeView odpowiadającego elementowi.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Inicjalizuje instancję klasy SelectedItem przy pomocy przyjaznej nazwy, wartości MDX i ścieżki do węzła w TreeView.
        /// </summary>
        /// <param name="name">Przyjazna nazwa.</param>
        /// <param name="value">Wartość w języku MDX.</param>
        /// <param name="path">Ścieżka do węzła kontrolki typu TreeView odpowiadającego elementowi.</param>
        protected SelectedItem(string name, string value, string path)
        {
            Name = name;
            Value = value;
            Path = path;
        }
    }
}