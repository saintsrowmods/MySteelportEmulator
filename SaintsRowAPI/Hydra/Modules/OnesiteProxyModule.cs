using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra.DataTypes;
using SaintsRowAPI.Models;

namespace SaintsRowAPI.Hydra.Modules
{
    public class OnesiteProxyModule : IModule
    {
        private HydraConnection Connection;

        public OnesiteProxyModule(HydraConnection connection)
        {
            Connection = connection;
        }

        public void HandleRequest(HydraRequest request)
        {
            switch (request.Action)
            {
                case "create":
                    {
                        Create(request);
                        break;
                    }
                case "display_user":
                    {
                        DisplayUser(request);
                        break;
                    }
                case "get_by_name":
                    {
                        GetByName(request);
                        break;
                    }
                case "get_by_platform_account_id":
                    {
                        GetByPlatformAccountId(request);
                        break;
                    }
                case "login_with_auth_user_session":
                    {
                        LoginWithAuthUserSession(request);
                        break;
                    }
                case "set_platform_account_id":
                    {
                        SetPlatformAccountId(request);
                        break;
                    }

                default:
                    {
                        request.DumpToFile();
                        break;
                    }
            }
        }

        private void Create(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);

            HydraHashMap map = item as HydraHashMap;

            string username = (map.Items["username"] as HydraUtf8String).Value;
            string password = (map.Items["password"] as HydraUtf8String).Value;
            string email = (map.Items["email"] as HydraUtf8String).Value;
            string firstname = (map.Items["firstname"] as HydraUtf8String).Value; // always contains "firstname"
            string lastname = (map.Items["lastname"] as HydraUtf8String).Value; // always contains "lastname"
            int birthDay = int.Parse((map.Items["birth_day"] as HydraUtf8String).Value); // always 0
            int birthMonth = int.Parse((map.Items["birth_month"] as HydraUtf8String).Value); // always 0
            int birthYear = int.Parse((map.Items["birth_year"] as HydraUtf8String).Value); // always 2000

            int userId = 1;

            HydraInt32 result = new HydraInt32(userId);
            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }

        private void DisplayUser(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);

            HydraHashMap map = item as HydraHashMap;
            HydraInt32 userId = map.Items["user_id"] as HydraInt32;

                HydraHashMap result = new HydraHashMap(new Dictionary<string, IHydraItem>()
                {
                    { "username", new HydraUtf8StringAsBinary("user") }
                });
            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }

        private void GetByName(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);

            HydraHashMap map = item as HydraHashMap;
            HydraUtf8String username = map.Items["username"] as HydraUtf8String;


            int userId = 1;

            HydraInt32 result = new HydraInt32(userId);
            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }

        private void GetByPlatformAccountId(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);

            HydraHashMap map = item as HydraHashMap;
            HydraUtf8String platformAccountIdString = map.Items["value"] as HydraUtf8String;
            ulong platformAccountId = ulong.Parse(platformAccountIdString.Value);

            HydraHashMap result = new HydraHashMap(new Dictionary<string, IHydraItem>()
            {
                { "getUsersByProperty", new HydraHashMap(new Dictionary<string, IHydraItem>() {
                    { "result", new HydraHashMap(new Dictionary<string, IHydraItem>() {
                        { "items", new HydraHashMap(new Dictionary<string, IHydraItem>() {
                            { "item", new HydraUtf8String("1") }
                        })}
                    })}
                })}
            });

            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }

        private void LoginWithAuthUserSession(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);
            HydraHashMap map = item as HydraHashMap;
            string username = (map.Items["username"] as HydraUtf8String).Value;
            string password = (map.Items["password"] as HydraUtf8String).Value;

            HydraHashMap result = new HydraHashMap(new Dictionary<string, IHydraItem>()
            {
                { "auth_user_return", new HydraUtf8StringAsBinary("success") },
                { "user_id", new HydraInt32(1) }
            });

            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }

        private void SetPlatformAccountId(HydraRequest request)
        {
            IHydraItem item = HydraItemDeserializer.Deserialize(request.PostData);
            HydraHashMap map = item as HydraHashMap;
            int userId = int.Parse((map.Items["userID"] as HydraUtf8String).Value);
            ulong accountId = ulong.Parse((map.Items["value"] as HydraUtf8String).Value);

            HydraHashMap result = new HydraHashMap(new Dictionary<string, IHydraItem>()
            {
                { "setProperty", new HydraHashMap(new Dictionary<string, IHydraItem>()
                    {
                        { "status", new HydraHashMap(new Dictionary<string, IHydraItem>()
                            {
                                { "code", new HydraUtf8String("1") }
                            }
                        )}
                    })
                }
            });

            HydraResponse response = new HydraResponse(Connection, result);
            response.Send();
        }
    }
}
