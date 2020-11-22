using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerCharacterView : MonoBehaviour
    {
        public PlayerCharacter character;

        private PlayerInterpolation interpolation;

        private void Start()
        {
            interpolation = new PlayerInterpolation(transform);
        }

        private void OnEnable()
        {
            character.Updated += Character_Updated;
        }

        private void OnDisable()
        {
            character.Updated -= Character_Updated;
        }

        private void Update()
        {
            interpolation.Update(character.tick);
        }

        private void Character_Updated(PlayerCharacter sender, PlayerUpdateData e)
        {
            interpolation.UpdateData(e, character.tick);
        }
    }
}
