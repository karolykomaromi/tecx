namespace TecX.Playground.Copy
{
    public class CopyContext
    {
        private readonly Copier _Copier;

        public CopyContext(Copier _Copier)
        {
            this._Copier = _Copier;
        }

        public Copier Copier
        {
            get
            {
                return this._Copier;
            }
        }
    }
}