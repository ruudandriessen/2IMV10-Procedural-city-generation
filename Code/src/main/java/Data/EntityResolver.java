/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Data;

import de.topobyte.osm4j.core.model.iface.OsmNode;
import de.topobyte.osm4j.core.model.iface.OsmRelation;
import de.topobyte.osm4j.core.model.iface.OsmWay;
import de.topobyte.osm4j.core.resolve.EntityNotFoundException;
import de.topobyte.osm4j.core.resolve.OsmEntityProvider;
import java.util.List;
import java.util.Map;

/**
 *
 * @author rikschreurs
 */
public class EntityResolver implements OsmEntityProvider {

    private Map<Long, OsmNode> nodes;
    private Map<Long, OsmWay> ways;
    private Map<Long, OsmRelation> relations;

    public EntityResolver(Map<Long, OsmNode> nodes, Map<Long, OsmWay> ways, Map<Long, OsmRelation> relations) {
        this.nodes = nodes;
        this.ways = ways;
        this.relations = relations;

    }

    @Override
    public OsmNode getNode(long l) throws EntityNotFoundException {
        if(!nodes.containsKey(l)) {
            throw new EntityNotFoundException("Node not found: " + l);
        }
        return nodes.get(l);
    }

    @Override
    public OsmWay getWay(long l) throws EntityNotFoundException {
        if(!ways.containsKey(l)) {
            throw new EntityNotFoundException("Way not found: " + l);
        }
        return ways.get(l);
    }

    @Override
    public OsmRelation getRelation(long l) throws EntityNotFoundException {
        if(!relations.containsKey(l)) {
            throw new EntityNotFoundException("Relation not found: " + l);
        }
        return relations.get(l);
    }

}
