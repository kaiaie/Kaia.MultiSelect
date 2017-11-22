using Kaia.Common.DataAccess.Contract;
using System;

namespace Kaia.Common.DataAccess
{
    public sealed class UpdatableField<T> : IIndeterminate, IUpdatable
    {
        private Indeterminate<T> _oldValue;
        private T _newValue;
        private bool _isUpdatable = true;
        private bool _isUpdated;

        public bool IsIndeterminate
        {
            get
            {
                return _oldValue.IsIndeterminate;
            }
        }


        public bool IsUpdated
        {
            get
            {
                return _isUpdated;
            }
        }

        public bool IsUpdatable
        {
            get
            {
                return _isUpdatable;
            }
        }


        public T Value
        {
            get
            {
                return _isUpdated ? _newValue : _oldValue.Value;
            }
            set
            {
                if (_isUpdatable)
                {
                    _newValue = value;
                    _isUpdated = true;
                }
                else
                {
                    throw new Exception("Value cannot be updated");
                }
            }
        }


        public UpdatableField(Indeterminate<T> oldValue, bool isUpdatable = true)
        {
            _oldValue = oldValue;
            _isUpdatable = isUpdatable;
        }


        public UpdatableField(T oldValue, bool isUpdatable = true) : 
            this(new Indeterminate<T>(oldValue), isUpdatable)
        {

        }


        public UpdatableField(bool isUpdatable = true) :
            this(new Indeterminate<T>(), isUpdatable)
        {

        }

    }
}
