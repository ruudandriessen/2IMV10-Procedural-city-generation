using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System;
using ProceduralCity;

public class LoadData {


	// Use this for initialization
	public static void Load () {
		using(XmlTextReader reader = new XmlTextReader("Assets/Maps/herpt2"))
//		using (XmlReader reader = XmlReader.Create("Assets/Maps/andorra"))
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
						float lat = float.Parse (reader ["lat"]);
						float lon = float.Parse (reader ["lon"]);

						List<OsmTag> nodeTags = new List<OsmTag> ();
						XmlReader nodeSubtree = reader.ReadSubtree ();
						readNodeSubtree (nodeSubtree, nodeTags);
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
						List<OsmTag> wayTags = new List<OsmTag> ();
						List<OsmNodeReference> wayNodes = new List<OsmNodeReference> ();
						XmlReader waySubtree = reader.ReadSubtree ();
						readWaySubtree (waySubtree, wayNodes, wayTags);
						OsmWay way = new OsmWay (wayId, wayTags, wayNodes);
						foreach (OsmTag tag in wayTags) {
							if (tag.getKey () == "building") {
								Data.Instance.buildings.Add (wayId, new OsmBuilding (wayId, wayTags, wayNodes));
							}
							if (tag.getKey () == "highway") {
								Data.Instance.streets.Add (wayId, new OsmStreet (wayId, wayTags, wayNodes));
							}
							if (tag.getKey () == "landuse" || tag.getKey() == "natural"  ){ //|| tag.getKey() == "surface") {
								if (!Data.Instance.surfaces.ContainsKey(wayId))
									Data.Instance.surfaces.Add (wayId, new OsmSurface (wayId, wayTags, wayNodes, tag.getValue()));
							}
						}
						Data.Instance.ways.Add(wayId, way);
						break;
					case "relation": 
						long relationId = long.Parse (reader ["id"]);
						List<OsmRelationMember> members = new List<OsmRelationMember> ();
						List<OsmTag> relationTags = new List<OsmTag> ();
						XmlReader relationSubtree = reader.ReadSubtree ();
						readRelationSubtree (relationSubtree, members, relationTags);
						OsmRelation relation = new OsmRelation (relationId, relationTags, members);
						Data.Instance.relations.Add(relationId, relation);
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
				if (!Data.Instance.nodes.ContainsKey (nodeRef.getId ()))
					continue;
				OsmNode n = (OsmNode) Data.Instance.nodes [nodeRef.getId()];
				n.addContainedIn (w);
				nodeRef.setLattitudeAndLongitude (n.getLatitude (), n.getLongitude ());
			}
		}
		Debug.Log ("Nodes: " + Data.Instance.nodes.Count);
		Debug.Log ("Ways: " + Data.Instance.ways.Count);
		Debug.Log ("Relations: " + Data.Instance.relations.Count);
		Debug.Log("Buildings: " + Data.Instance.buildings.Count);
		Debug.Log("Streets: " + Data.Instance.streets.Count);
			
		Debug.Log ("Done");
		Data.Instance.dataLoaded = true;
	}

	private static void readWaySubtree(XmlReader subtree, List<OsmNodeReference> references, List<OsmTag> tags) {
		while (subtree.Read ()) {
			switch (subtree.Name) {
			case "tag":
				tags.Add (new OsmTag (subtree ["k"], subtree ["v"]));
				break;
			case "nd":
				references.Add (new OsmNodeReference (long.Parse (subtree ["ref"])));
				break;
			}
		}
	}

	private static void readRelationSubtree(XmlReader subtree, List<OsmRelationMember> members, List<OsmTag> tags) {
		while (subtree.Read ()) {
			switch (subtree.Name) {
			case "member":
				EntityType type = EntityTypeMethods.fromString (subtree ["type"]);
				members.Add (new OsmRelationMember (long.Parse (subtree ["ref"]), type, subtree ["role"]));
				break;
			case "tag":
				tags.Add (new OsmTag (subtree ["k"], subtree ["v"]));
				break;
			}
		}
	}
	private static void readNodeSubtree(XmlReader subtree, List<OsmTag> tags) {
		while (subtree.Read ()) {
			switch (subtree.Name) {
			case "tag":
				tags.Add (new OsmTag (subtree ["k"], subtree ["v"]));
				break;
			}
		}
	}
}
