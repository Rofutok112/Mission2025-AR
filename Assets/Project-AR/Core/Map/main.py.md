このファイルは参考用です。動かしません。

```python
import xml.etree.ElementTree as ET
from fastapi import FastAPI
import math

app = FastAPI()

tree = ET.parse("map.xml")
root = tree.getroot()

# 二点間距離（メートル）
def distance_m(lat1, lon1, lat2, lon2):
    # 地球の半径
    R = 6371000
    d_lat = math.radians(lat2 - lat1)
    d_lon = math.radians(lon2 - lon1)
    a = math.sin(d_lat/2)**2 + math.cos(math.radians(lat1)) * math.cos(math.radians(lat2)) * math.sin(d_lon/2)**2
    return R * 2 * math.atan2(math.sqrt(a), math.sqrt(1-a))

@app.get("/search")
def search(lat: float, lon: float, radius: int = 200):
    results = []
    for node in root.findall("node"):
        nlat = float(node.attrib["lat"])
        nlon = float(node.attrib["lon"])
        d = distance_m(lat, lon, nlat, nlon)
        if d <= radius:
            results.append({
                "id": node.attrib["id"],
                "lat": nlat,
                "lon": nlon,
                "distance": d
            })
    return {"nodes": results}
```