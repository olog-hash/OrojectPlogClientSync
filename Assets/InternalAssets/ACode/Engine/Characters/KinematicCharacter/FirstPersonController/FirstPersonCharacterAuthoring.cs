using ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
    public class FirstPersonCharacterAuthoring : MonoBehaviour
    {
        public EntityProvider CharacterViewTransform;
        public CapsuleCollider CapsuleCollider;
        public AuthoringKinematicCharacterBody CharacterBody = AuthoringKinematicCharacterBody.GetDefault();
        public FirstPersonCharacter FirstPersonCharacter = FirstPersonCharacter.GetDefault();

        void Start()
        {
            if (CharacterViewTransform == null)
            {
                Debug.LogError("ERROR: the CharacterViewTransform must not be null. You must assign a 1st-level child object of the character to this field (the object that represents the camera point). Conversion will be aborted");
                return;
            }
            if (CharacterViewTransform.transform.parent != this.transform)
            {
                Debug.LogError("ERROR: the CharacterViewTransform must be a direct 1st-level child of the character authoring GameObject. Conversion will be aborted");
                return;
            }

            if (TryGetComponent(out EntityProvider provider))
            {
                FirstPersonCharacter.CharacterViewEntity = CharacterViewTransform;
                CharacterBody.Capsule = CapsuleCollider;

                provider.AddComponentData(new Translation { Transform = transform });
                provider.AddComponentData(new FirstPersonInputs());
                provider.AddComponentData(FirstPersonCharacter);

                KinematicCharacterUtilities.HandleConversionForCharacter(provider.Entity, gameObject, CharacterBody);

                Destroy(this);
            }
        }
    }
}
