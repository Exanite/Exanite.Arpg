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
            character.StateUpdated += Character_StateUpdated;
        }

        private void OnDisable()
        {
            character.StateUpdated -= Character_StateUpdated;
        }

        private void Update()
        {
            interpolation.Update(character.Time);
        }

        private void Character_StateUpdated(PlayerCharacter sender, PlayerStateData e)
        {
            interpolation.UpdateData(e, character.Time.CurrentTick);
        }
    }
}
