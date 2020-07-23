using Exanite.Arpg.Networking.Shared;
using UnityEngine;

namespace Exanite.Arpg.Networking.Server.Authentication
{
    public class Authenticator
    {
        private readonly PlayerManager playerManager;

        public Authenticator(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public AuthenticationResult Authenticate(LoginRequest request)
        {
            var result = new AuthenticationResult()
            {
                IsSuccess = true,
            };

            if (Application.version != request.GameVersion)
            {
                result.IsSuccess = false;
                result.FailReason = $"Client game version '{request.GameVersion}' did not match server game version '{Application.version}'";
            }
            else if (playerManager.Contains(request.PlayerName))
            {
                result.IsSuccess = false;
                result.FailReason = $"Player with name {request.PlayerName} already exists on the server";
            }

            return result;
        }
    }
}
