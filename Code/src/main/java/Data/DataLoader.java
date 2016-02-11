/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Data;

import com.vividsolutions.jts.geom.Geometry;
import com.vividsolutions.jts.geom.LineString;
import com.vividsolutions.jts.geom.MultiPolygon;
import de.topobyte.osm4j.core.access.OsmInputException;
import de.topobyte.osm4j.core.access.OsmIterator;
import de.topobyte.osm4j.core.model.iface.EntityContainer;
import de.topobyte.osm4j.core.model.iface.OsmNode;
import de.topobyte.osm4j.core.model.iface.OsmRelation;
import de.topobyte.osm4j.core.model.iface.OsmWay;
import de.topobyte.osm4j.core.model.util.OsmModelUtil;
import de.topobyte.osm4j.core.resolve.EntityFinder;
import de.topobyte.osm4j.core.resolve.EntityFinders;
import de.topobyte.osm4j.core.resolve.EntityNotFoundException;
import de.topobyte.osm4j.core.resolve.EntityNotFoundStrategy;
import de.topobyte.osm4j.geometry.RegionBuilder;
import de.topobyte.osm4j.geometry.RegionBuilderResult;
import de.topobyte.osm4j.geometry.WayBuilder;
import de.topobyte.osm4j.geometry.WayBuilderResult;
import de.topobyte.osm4j.pbf.seq.PbfIterator;
import java.io.IOException;
import java.io.InputStream;
import java.net.MalformedURLException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 *
 * @author rikschreurs
 */
public class DataLoader {

    private WayBuilder wayBuilder = new WayBuilder();
    private RegionBuilder regionBuilder = new RegionBuilder();

    private EntityResolver resolver;
    Set<OsmWay> buildingRelationWays = new HashSet<>();
    // We use this to find all way members of relations.
    EntityFinder wayFinder;

    private List<MultiPolygon> buildings = new ArrayList<>();
    private List<LineString> streets = new ArrayList<>();
    private Map<LineString, String> names = new HashMap<>();
    private Set<String> validHighways = new HashSet<>(
            Arrays.asList(new String[]{"primary", "secondary", "tertiary",
        "residential", "living_street"}));

    public DataLoader() {
        InputStream input = DataLoader.class.getClassLoader().getResourceAsStream("maps/andorra.osm.pbf");
        OsmIterator iterator = new PbfIterator(input, true);
        Map<Long, OsmNode> nodes = new HashMap<>();
        Map<Long, OsmWay> ways = new HashMap<>();
        Map<Long, OsmRelation> relations = new HashMap<>();
        for (EntityContainer container : iterator) {
            switch (container.getType()) {
                default:
                case Node:
                    nodes.put(container.getEntity().getId(), (OsmNode) container.getEntity());
                    break;
                case Way:
                    ways.put(container.getEntity().getId(), (OsmWay) container.getEntity());
                    break;
                case Relation:
                    relations.put(container.getEntity().getId(), (OsmRelation) container.getEntity());
                    break;
            }
        }
        loadData(nodes, ways, relations);
    }
    
    public List<MultiPolygon> getMultiPolygons() {
        return this.buildings;
    }
    
    public List<LineString> getRoads() {
        return this.streets;
    }
    
    private void loadData(Map nodes, Map ways, Map relations) {
        resolver = new EntityResolver(nodes, ways, relations);
        wayFinder = EntityFinders.create(resolver, EntityNotFoundStrategy.IGNORE);
        getBuildings(relations, ways);
        getStreets(ways);
        outputResults();
    }

    private Collection<LineString> getLine(OsmWay way) {
        List<LineString> results = new ArrayList<>();
        try {
            WayBuilderResult lines = wayBuilder.build(way, resolver);
            results.addAll(lines.getLineStrings());
            if (lines.getLinearRing() != null) {
                results.add(lines.getLinearRing());
            }
        } catch (EntityNotFoundException e) {
            // ignore
        }
        return results;
    }

    private MultiPolygon getPolygon(OsmWay way) {
        try {
            RegionBuilderResult region = regionBuilder.build(way, resolver);
            return region.getMultiPolygon();
        } catch (EntityNotFoundException e) {
            return null;
        }
    }

    private MultiPolygon getPolygon(OsmRelation relation) {
        try {
            RegionBuilderResult region = regionBuilder.build(relation, resolver);
            return region.getMultiPolygon();
        } catch (EntityNotFoundException e) {
            return null;
        }
    }

    private void outputResults() {
        System.out.println("Buildings loaded: " + buildings.size());
        System.out.println("Streets loaded: " + streets.size());
        System.out.println("Names loaded: " + names.size());
    }

    private void getBuildings(Map<Long, OsmRelation> relations, Map<Long, OsmWay> ways) {
        for (Long key : relations.keySet()) {
            OsmRelation relation = relations.get(key);
            Map<String, String> tags = OsmModelUtil.getTagsAsMap(relation);
            if (tags.containsKey("building")) {
                MultiPolygon area = getPolygon(relation);
                if (area != null) {
                    buildings.add(area);
                }
                try {
                    wayFinder.findMemberWays(relation, buildingRelationWays);
                } catch (EntityNotFoundException e) {
                    // cannot happen (IGNORE strategy)
                }
            }
        }

        for (Long key : ways.keySet()) {
            OsmWay way = ways.get(key);
            Map<String, String> tags = OsmModelUtil.getTagsAsMap(way);
            if (tags.containsKey("building")) {
                MultiPolygon area = getPolygon(way);
                if (area != null) {
                    buildings.add(area);
                }
            }
        }
    }

    private void getStreets(Map<Long, OsmWay> ways) {
        for (Long key : ways.keySet()) {
            OsmWay way = ways.get(key);
            Map<String, String> tags = OsmModelUtil.getTagsAsMap(way);
            String highway = tags.get("highway");
            if (!(highway == null)) {
                Collection<LineString> paths;

                paths = getLine(way);

                if (!(!validHighways.contains(highway))) {
                    // Okay, this is a valid street
                    paths.stream().forEach((path) -> {
                        streets.add(path);
                    }); // If it has a name, store it for labeling
                    String name = tags.get("name");
                    if (!(name == null)) {
                        paths.stream().forEach((path) -> {
                            names.put(path, name);
                        });
                    }
                }

            }
        }

    }

}
