using Kaia.Common.DataAccess.Contract;

namespace Kaia.Common.DataAccess
{
    public sealed class UpdatableField<T> : IIndeterminate, IUpdatable
    {
        private Indeterminate<T> _oldValue;
        private T _newValue;
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


        public T Value
        {
            get
            {
                return _isUpdated ? _newValue : _oldValue.Value;
            }
            set
            {
                _newValue = value;
                _isUpdated = true;
            }
        }


        public UpdatableField(Indeterminate<T> oldValue)
        {
            _oldValue = oldValue;
        }

        public UpdatableField(T oldValue) : this(new Indeterminate<T>(oldValue))
        {

        }
    }
}
