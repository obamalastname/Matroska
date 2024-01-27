using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Matroska.Tools;
using UnityEngine;

namespace Matroska.Tools.Audio.Examples
{
	public class FMODEventsExample : MonoSingleton<FMODEventsExample>
	{
		[field: Header("Example")]
		[field: SerializeField] public EventReference exampleSoundTriggered { get; private set; }
	}
}