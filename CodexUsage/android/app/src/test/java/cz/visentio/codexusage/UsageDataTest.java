package cz.visentio.codexusage;
import org.junit.Test;
import org.json.*;
import java.time.ZoneId;
import static org.junit.Assert.*;

public class UsageDataTest {
    private JSONObject window(double used,long seconds,long reset)throws Exception{
        return new JSONObject().put("used_percent",used).put("limit_window_seconds",seconds).put("reset_at",reset);
    }
    private JSONObject payload(JSONObject primary,JSONObject secondary)throws Exception{
        JSONObject limits=new JSONObject();if(primary!=null)limits.put("primary_window",primary);if(secondary!=null)limits.put("secondary_window",secondary);
        return new JSONObject().put("rate_limit",limits);
    }
    @Test public void ordersWindowsByDurationNotPosition()throws Exception{
        JSONObject result=UsageData.parse(payload(window(41,604800,1760148000),window(47,18000,1760148000)),100);
        assertEquals("53%",UsageData.percent(result,"short"));assertEquals("59%",UsageData.percent(result,"week"));
    }
    @Test public void missingShortLimitIsNeverZeroOrFull()throws Exception{
        JSONObject result=UsageData.parse(payload(window(23,604800,0),null),100);
        assertEquals("—",UsageData.percent(result,"short"));assertEquals("77%",UsageData.percent(result,"week"));
    }
    @Test public void timeUses24HourClock()throws Exception{
        long reset=java.time.Instant.parse("2026-10-09T21:07:00Z").getEpochSecond();
        JSONObject result=UsageData.parse(payload(window(10,18000,reset),null),100);
        assertEquals("23:07",UsageData.reset(result,"short",ZoneId.of("Europe/Prague")));
    }
    @Test public void cacheIsMarkedOldAndFutureTimestampInvalid()throws Exception{
        JSONObject result=UsageData.parse(payload(window(10,18000,0),null),100);
        assertFalse(UsageData.stale(result,110));assertTrue(UsageData.stale(result,1301));assertTrue(UsageData.stale(result,99));
    }
    @Test(expected=Exception.class) public void rejectInvalidPercent()throws Exception{UsageData.parse(payload(window(101,18000,0),null),100);}
    @Test(expected=Exception.class) public void rejectUnknownOnlyWindow()throws Exception{UsageData.parse(payload(window(12,3600,0),null),100);}
    @Test(expected=Exception.class) public void rejectNegativeReset()throws Exception{UsageData.parse(payload(window(12,18000,-1),null),100);}
    @Test(expected=Exception.class) public void rejectEmptyResponse()throws Exception{UsageData.parse(new JSONObject(),100);}
}
