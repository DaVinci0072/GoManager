using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using POGOProtos.Enums;

namespace PokemonGo.RocketAPI.Rpc
{
    public class Misc : BaseRpc
    {
        public Misc(Client client) : base(client)
        {
        }


        public async Task<ClaimCodenameResponse> ClaimCodename(string codename)
        {
            return
                await
                    PostProtoPayload<Request, ClaimCodenameResponse>(RequestType.ClaimCodename,
                        new ClaimCodenameMessage()
                        {
                            Codename = codename
                        }).ConfigureAwait(false);
        }

        public async Task<CheckCodenameAvailableResponse> CheckCodenameAvailable(string codename)
        {
            return
                await
                    PostProtoPayload<Request, CheckCodenameAvailableResponse>(RequestType.CheckCodenameAvailable,
                        new CheckCodenameAvailableMessage()
                        {
                            Codename = codename
                        }).ConfigureAwait(false);
        }

        public async Task<GetSuggestedCodenamesResponse> GetSuggestedCodenames()
        {
            return await PostProtoPayload<Request, GetSuggestedCodenamesResponse>(RequestType.GetSuggestedCodenames, new GetSuggestedCodenamesMessage()).ConfigureAwait(false);
        }

        public async Task<EchoResponse> SendEcho()
        {
            return await PostProtoPayload<Request, EchoResponse>(RequestType.Echo, new EchoMessage()).ConfigureAwait(false);
        }

        public async Task<MarkTutorialCompleteResponse> MarkTutorialComplete(List<TutorialState> tutorials)
        {
            MarkTutorialCompleteMessage message = new MarkTutorialCompleteMessage();

            message.SendPushNotifications = false;
            message.SendMarketingEmails = false;

            foreach(TutorialState tutorial in tutorials)
            {
                message.TutorialsCompleted.Add(tutorials);
            }

            return await PostProtoPayload<Request, MarkTutorialCompleteResponse>(RequestType.MarkTutorialComplete, message).ConfigureAwait(false);
        }
    }
}