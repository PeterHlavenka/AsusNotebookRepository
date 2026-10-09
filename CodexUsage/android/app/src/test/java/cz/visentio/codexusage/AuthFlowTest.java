package cz.visentio.codexusage;
import org.junit.Test;
import static org.junit.Assert.*;

public class AuthFlowTest {
    @Test public void pkceMatchesRfc7636Vector()throws Exception{
        assertEquals("E9Melhoa2OwvFrEMTJguCHaoeK1t8URWbuGJSstw-cM",AuthFlow.challenge("dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk"));
    }
    @Test public void rejectMissingOrWrongCallbackState(){
        assertFalse(AuthFlow.matches("valid",null));assertFalse(AuthFlow.matches("valid","other"));
        assertFalse(AuthFlow.matches("",""));assertTrue(AuthFlow.matches("valid","valid"));
    }
    @Test public void randomVerifiersAreDistinctAndUrlSafe(){
        String a=AuthFlow.random(),b=AuthFlow.random();assertNotEquals(a,b);assertEquals(43,a.length());assertTrue(a.matches("[A-Za-z0-9_-]{43}"));
    }
}
