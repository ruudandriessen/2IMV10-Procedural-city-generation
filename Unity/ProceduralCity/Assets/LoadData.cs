using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System;
using AssemblyCSharp;

public class LoadData : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//XmlTextReader reader = new XmlTextReader("Assets/andorra");
		using (XmlReader reader = XmlReader.Create("Assets/map"))
		{
			while (reader.Read())
			{
				// Only detect start elements.
				if (reader.IsStartElement())
				{
					//Debug.Log (reader.Name);
					// Get element name and switch on it.
					switch (reader.Name)
					{
					case "osm":
						// Detect this element.
						break;
					case "bounds":
						break;
					case "node":
						long nodeId = long.Parse (reader ["id"]);
						double lat = double.Parse (reader ["lat"]);
						double lon = double.Parse (reader ["lon"]);

						List<OsmTag> nodeTags = new List<OsmTag> ();
						if (reader.ReadToDescendant ("tag")) {
							nodeTags.Add (new OsmTag (reader ["k"], reader ["v"]));
							while (reader.ReadToNextSibling ("tag")) {
								nodeTags.Add (new OsmTag (reader ["k"], reader ["v"]));
							}
						}
						OsmNode node = new OsmNode (nodeId, nodeTags, lon, lat);
						Data.Instance.nodes.Add (nodeId, node);
						break;
					case "tag":
						break;
					case "member": 
						break;
					case "nd":
						break;
					case "way":
						long wayId = long.Parse (reader ["id"]);
						List<OsmNodeReference> wayNodes = new List<OsmNodeReference> ();
						if (reader.ReadToDescendant ("nd")) {
							wayNodes.Add (new OsmNodeReference(long.Parse (reader ["ref"])));
							while (reader.ReadToNextSibling ("nd")) {
								wayNodes.Add (new OsmNodeReference(long.Parse (reader ["ref"])));
							}
						}
						List<OsmTag> wayTags = new List<OsmTag> ();
						if (reader.ReadToDescendant ("tag")) {
							wayTags.Add (new OsmTag (reader ["k"], reader ["v"]));
							while (reader.ReadToNextSibling ("tag")) {
								wayTags.Add (new OsmTag (reader ["k"], reader ["v"]));
							}
						}
						OsmWay way = new OsmWay (wayId, wayTags, wayNodes);
						Data.Instance.ways.Add(wayId, way);
						break;
					case "relation": 
						break;
					default:
						Debug.Log (reader.Name);
						break;
					}


				}
			}
		}
		foreach(long key in Data.Instance.ways.Keys)
		{
			
			OsmWay w = (OsmWay) Data.Instance.ways[key];
			for(int i = 0; i < w.getNumberOfNodes(); i++) {
				OsmNodeReference nodeRef = w.getNodeReference (i);
				OsmNode n = (OsmNode) Data.Instance.nodes [nodeRef.getId()];
				nodeRef.setLattitudeAndLongitude (n.getLatitude (), n.getLongitude ());
			}
		}
		Debug.Log ("Nodes: " + Data.Instance.nodes.Count);
		Debug.Log ("Ways: " + Data.Instance.ways.Count);
		Debug.Log ("Done");
		Data.Instance.dataLoaded = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
