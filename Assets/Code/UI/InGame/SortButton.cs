using Items;

namespace UI
{
    public class SortButton : Button
    {
        private Container _container;

        public void Initialize(Container container)
        {
            _container = container;
        }

        protected override void Execute()
        {
            _container.Sort();
        }
    }
}