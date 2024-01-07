using Cinemachine;
using UnityEngine;

namespace Matroska.Tools 
{
	public class Boostrapper : MonoSingleton<Boostrapper>
	{
		[SerializeField] private CinemachineBrain cinemachineBrain;

		public CinemachineBrain CinemachineBrain => cinemachineBrain;
	}
}