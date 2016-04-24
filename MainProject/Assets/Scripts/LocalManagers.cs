using UnityEngine;
using System.Collections.Generic;

namespace Codium
{
	public class LocalManagers : MonoBehaviour {

		public GameObject[] managerTypes;

		private Dictionary<string, Dictionary<GameObject, bool>> m_managers;
		
		public Dictionary<string, Dictionary<GameObject, bool>> GetManagers () {
			if (m_managers == null)
				m_managers = new Dictionary<string, Dictionary<GameObject, bool>>();
				
			return m_managers;
		}
		
		public void SetManagers (Dictionary<string, Dictionary<GameObject, bool>> managers) {
			m_managers = managers;
		}
		
	}	
}
