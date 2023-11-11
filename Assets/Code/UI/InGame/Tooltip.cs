using System;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class Tooltip : IEquatable<Tooltip>
    {
        public TooltipElement[] _elements = new TooltipElement[0];

        public TooltipElement this[int i]
        {
            get => _elements[i];
            set => _elements[i] = value;
        }

        public IEnumerable<TooltipElement> Elements => _elements;

        public Tooltip(params TooltipElement[] elements)
        {
            _elements = elements;
        }

        public Tooltip Merge(Tooltip other)
        {
            return new(Elements.Concat(other.Elements).ToArray());
        }

        public static Tooltip Merge(params Tooltip[] tooltips)
        {
            TooltipElement[] elements = new TooltipElement[0];
            foreach (Tooltip tooltip in tooltips)
            {
                elements.Concat(tooltip.Elements);
            }
            return new(elements);
        }

        public override int GetHashCode()
        {
            return Elements.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not Tooltip)
                return false;
            return Equals((Tooltip)obj);
        }

        public bool Equals(Tooltip other)
        {
            return other.Elements.SequenceEqual(Elements);
        }

        public static bool operator ==(Tooltip lhs, Tooltip rhs)
        {
            if (lhs is null)
                return rhs is null;
            if (rhs is null)
                return lhs is null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Tooltip lhs, Tooltip rhs)
        {
            return !(lhs == rhs);
        }
    }
}