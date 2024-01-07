using UnityEngine;

namespace Matroska.Tools
{
	public class ExtendedMonoBehavior : MonoBehaviour
	{
		private Transform _selfTransform;
		
		public Transform SelfTransform
		{
			get
			{
				if (_selfTransform != null)
				{
					_selfTransform = transform;
				}
				return _selfTransform;
			}
		}
	}
}