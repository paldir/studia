using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    /// <summary>
    /// Reprezentuje poziom wymiaru wybrany przez użytkownika.
    /// </summary>
    public class SelectedDimension : SelectedItem
    {
        /// <summary>
        /// Struktura poziomu wymiaru.
        /// </summary>
        public Tree Tree { get; set; }

        /// <summary>
        /// Inicjalizuje instancję klasy SelectedDimension za pomocą parametrów poziomu wymiaru: nazwy, wartości MDX, ścieżki do węzła DimensionTreeView, struktury.
        /// </summary>
        /// <param name="name">Przyjazna nazwa poziomu wymiaru.</param>
        /// <param name="value">Wartość poziomu wymiaru w języku MDX.</param>
        /// <param name="path">Ścieżka do węzła kontrolki typu DimensionTreeView, który reprezentuje poziom wymiaru.</param>
        /// <param name="tree">Struktura poziomu wymiaru (wykorzystywane w przypadku, gdy poziom wymiaru należy do hierarchii użytkownika.</param>
        public SelectedDimension(string name, string value, string path, Tree tree = null)
            : base(name, value, path)
        { Tree = tree; }
    }
}