using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Matroska.Tools.Audio.Examples
{
	public class AudioTriggersExample : MonoBehaviour
	{
		[Sirenix.OdinInspector.Button]
		public void PlayOneShot()
		{
			AudioManagerExample.Instance.PlayOneShot(FMODEventsExample.Instance.exampleSoundTriggered, this.transform.position);
		}
	}
}