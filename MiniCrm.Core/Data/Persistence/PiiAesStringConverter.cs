using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PiiTypes;

namespace MiniCrm.Core.Data.Persistence
{
    public class PiiAesStringConverter : ValueConverter<PiiString, string>
    {
        private static IPiiEncoder s_encoder = null!;
        private static readonly Lazy<PiiAesStringConverter> Instance = new();
        public static PiiAesStringConverter Default => Instance.Value;

        public PiiAesStringConverter() : base(
            v => s_encoder.ToSystemString(v),
            v => s_encoder.ToPiiString(v))
        {
            if (s_encoder == null)
            {
                throw new TypeInitializationException(GetType().FullName,
                    new InvalidOperationException("cannot create an instance until SetEncoder is called"));
            }
        }

        public static void SetEncoder(IPiiEncoder encoder)
        {
            s_encoder = encoder;
        }
    }
}
