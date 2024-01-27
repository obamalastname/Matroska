using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Matroska.Tools.Audio.Examples
{
	public class AudioManagerExample : MonoSingleton<AudioManagerExample>
	{
		public void PlayOneShot(EventReference sound, Vector3 worldPos)
		{
			RuntimeManager.PlayOneShot(sound, worldPos);
		}
	}
}