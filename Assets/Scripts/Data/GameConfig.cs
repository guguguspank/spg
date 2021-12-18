namespace Spg
{
    public class GameConfig
    {
        private int? _configVersion;
        private bool? _extra;
        private int? _girdCount;

        public int ConfigVersion
        {
            get => _configVersion ?? 0;
            set => _configVersion = value;
        }

        public bool Extra
        {
            get => _extra ?? false;
            set => _extra = value;
        }

        public int GirdCount
        {
            get => _girdCount ?? 50;
            set => _girdCount = value < 1 ? 1 : value > 1105 ? 1105 : value;
        }
    }
}