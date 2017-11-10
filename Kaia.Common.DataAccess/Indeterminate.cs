using Kaia.Common.DataAccess.Contract;
using System;

namespace Kaia.Common.DataAccess
{
    public sealed class Indeterminate<T> : IIndeterminate
    {
        private T _value;
        private bool _isIndeterminate;

        public Indeterminate()
        {
            _isIndeterminate = true;
        }

        public Indeterminate(T initialValue)
        {
            _value = initialValue;
            _isIndeterminate = false;
        }

        public bool IsIndeterminate
        {
            get
            {
                return _isIndeterminate;
            }
        }

        public T Value
        {
            get
            {
                if (_isIndeterminate)
                {
                    throw new InvalidOperationException("Value is indeterminate");
                }
                return _value;
            }
            set
            {
                if (IsIndeterminate) return;
                if (!value.Equals(_value))
                {
                    _isIndeterminate = true;
                }
            }
        }
    }
}
