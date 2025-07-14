namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    public class SignalValueProperty<TValue>
    {
        public TValue defaultValue;
        public bool hasValue;
        public TValue value;

        public SignalValueProperty()
        {
            defaultValue = default;
            value = default;
        }

        public SignalValueProperty(TValue defaultValue)
        {
            this.defaultValue = defaultValue;
            value = defaultValue;
        }

        public bool Has => hasValue;

        public TValue Value
        {
            get => Take();
            set
            {
                this.value = value;
                hasValue = true;
            }
        }

        public void SetValue(TValue newValue)
        {
            value = newValue;
            hasValue = true;
        }

        public TValue Take()
        {
            if (!hasValue) return defaultValue;
            hasValue = false;
            var result = value;
            value = defaultValue;
            return result;
        }

        public bool Take(out TValue result)
        {
            if (!hasValue)
            {
                result = default;
                return false;
            }

            result = Take();
            return true;
        }

        public TValue Look() => value;
    }
}