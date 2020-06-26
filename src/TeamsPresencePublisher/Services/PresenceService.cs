using Microsoft.Graph;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TeamsPresencePublisher.Services
{
    public class PresenceService : IPresenceService
    {
        private GraphServiceClient _graphClient;

        public PresenceService(IAuthenticationProvider authProvider)
        {
            _graphClient = new GraphServiceClient(authProvider);
        }

        public async Task<Presence> GetPresenceAsync()
        {
            Presence presence = await _graphClient.Me.Presence.Request().GetAsync();
            return presence;

            //IUserRequest userRequest = _graphServiceClient.Me.Request();
            //IPresenceRequest presenceRequest = _graphServiceClient.Me.Presence.Request();

            //BatchRequestContent batchRequestContent = new BatchRequestContent();

            //var userRequestId = batchRequestContent.AddBatchRequestStep(userRequest);
            //var presenceRequestId = batchRequestContent.AddBatchRequestStep(presenceRequest);

            //BatchResponseContent returnedResponse = await _graphServiceClient.Batch.Request().PostAsync(batchRequestContent);

            //User user = await returnedResponse.GetResponseByIdAsync<User>(userRequestId);
            //Presence presence = await returnedResponse.GetResponseByIdAsync<Presence>(presenceRequestId);

            //return (User: user, Presence: presence);
        }

        public async Task<string> GetUsernameAsync()
        {
            User user = await _graphClient.Me.Request().GetAsync();
            return user.PreferredName ?? user.DisplayName;
        }

        public BitmapImage GetDefaultPhoto()
        {
            Uri uri = new Uri("pack://application:,,,/TeamsPresencePublisher;component/Assets/Portrait_Placeholder.png");
            return new BitmapImage(uri);
        }

        public async Task<BitmapImage> GetPhotoAsync()
        {
            BitmapImage result = null;

            try
            {
                Stream photo = await _graphClient.Me.Photo.Content.Request().GetAsync();
                if (photo != null)
                {
                    result = LoadImage(ReadFully(photo));
                }
            }
            catch
            {
            }

            return result ?? GetDefaultPhoto();
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                //image.SetSource(mem);
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            return image;

            //using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            //{
            //    DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0));
            //    writer.WriteBytes(imageData);
            //    await writer.StoreAsync();

            //    var image = new BitmapImage();
            //    await image.SetSourceAsync(stream);

            //    return image;
            //}
        }

    }
}
