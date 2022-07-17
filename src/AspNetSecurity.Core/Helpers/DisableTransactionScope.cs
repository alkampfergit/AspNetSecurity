using AspNetSecurity.Core.SqlHelpers;

namespace AspNetSecurity.Core.Helpers
{
    public class DisableTransactionScope : IDisposable
    {
        public static IDisposable Enter() => new DisableTransactionScope();

        private bool _actualValue;

        private DisableTransactionScope()
        {
            _actualValue = DataAccess.DisableTransaction.Value;
            DataAccess.DisableTransaction.Value = true;
        }

        public void Dispose()
        {
            DataAccess.DisableTransaction.Value = _actualValue;
        }
    }
}
