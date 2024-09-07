using QRCoder;

namespace Application.Helper
{
    public class QRCodeHelper
    {
        public static byte[] GenerateQRCode(string data)
        {
            var qRCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qRCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var bitmap = new BitmapByteQRCode(qrCodeData);
            var QRCode = bitmap.GetGraphic(20);

            return QRCode;
        }


    }
}
