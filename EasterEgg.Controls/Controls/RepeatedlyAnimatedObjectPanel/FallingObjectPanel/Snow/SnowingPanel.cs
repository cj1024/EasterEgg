using System.Windows.Media.Animation;

namespace EasterEgg.Controls
{

    public sealed class SnowingPanel : FallingObjectPanel<Snow>
    {

        public SnowingPanel()
        {
            DefaultStyleKey = typeof(SnowingPanel);
        }

        protected override Storyboard GenerateTransition(Snow target)
        {
            var result = base.GenerateTransition(target);
            return result;
        }

    }

}
