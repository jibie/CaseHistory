using OfIllness.ModeView;

namespace OfIllness
{
    public class AppGlobal
    {
        public static IDataEngine DataQuery = new DataEngine();

        public static ShowDetailsComm ShowPatientInfo = new ShowDetailsComm();

        public static ImageThumbnail ImgThumbnail = new ImageThumbnail();

        public static ShowPhoto ShowPho = new ShowPhoto();
    }
}
