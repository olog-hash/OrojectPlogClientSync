using ProjectOlog.Code.Engine.StateMachines.Interactables;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Objects.Interactables
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class InteractionObjectProvider : MonoProvider<InteractionObjectComponent> 
	{

	}

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct InteractionObjectComponent : IComponent 
	{
		public InteractionObjectStateManager ObjectStateManager;
	}
}