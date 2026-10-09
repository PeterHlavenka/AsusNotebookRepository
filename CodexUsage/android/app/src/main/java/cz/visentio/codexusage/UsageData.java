package cz.visentio.codexusage;

import org.json.*;
import java.time.*;
import java.time.format.DateTimeFormatter;
import java.util.Locale;

final class UsageData {
    static JSONObject parse(JSONObject payload, long now) throws Exception {
        JSONObject limits=payload.optJSONObject("rate_limit");
        if (limits==null) throw new Exception("Server nevrátil limity Codexu");
        JSONObject result=new JSONObject().put("observedAt",now);
        int count=0;
        for (String name : new String[]{"primary_window","secondary_window"}) {
            JSONObject window=limits.optJSONObject(name); if (window==null) continue;
            long duration=window.optLong("limit_window_seconds",-1);
            String slot=duration==18000?"short":duration==604800?"week":null;
            if(slot==null) continue;
            double used=window.getDouble("used_percent");
            if(!Double.isFinite(used)||used<0||used>100) throw new Exception("Neplatná procenta využití");
            long reset=window.optLong("reset_at",0);
            if(reset<0||reset>253402300799L) throw new Exception("Neplatný čas resetu");
            result.put(slot,new JSONObject().put("remaining",100-used).put("reset",reset)); count++;
        }
        if(count==0) throw new Exception("Pětihodinový ani týdenní limit není dostupný");
        return result;
    }
    static String percent(JSONObject snapshot, String slot) {
        JSONObject w=snapshot==null?null:snapshot.optJSONObject(slot);
        return w==null?"—":String.format(Locale.ROOT,"%.0f%%",w.optDouble("remaining"));
    }
    static String reset(JSONObject snapshot,String slot,ZoneId zone) {
        JSONObject w=snapshot==null?null:snapshot.optJSONObject(slot);
        if(w==null||w.optLong("reset")==0) return "—";
        return DateTimeFormatter.ofPattern(slot.equals("short")?"HH:mm":"d. M.",Locale.ROOT)
            .format(Instant.ofEpochSecond(w.optLong("reset")).atZone(zone));
    }
    static boolean stale(JSONObject snapshot,long now) {
        return snapshot==null||now-snapshot.optLong("observedAt")>1200||now<snapshot.optLong("observedAt");
    }
}
