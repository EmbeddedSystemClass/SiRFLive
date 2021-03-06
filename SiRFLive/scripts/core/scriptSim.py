import sys
import clr
clr.AddReferenceByPartialName('CommonClassLibrary')
from CommonClassLibrary import *
clr.AddReferenceByPartialName('SiRFLive')
from SiRFLive import *

import scriptGlobals
import scriptUtilities

def isSimEnd():
    if (scriptGlobals.MainFrame.SimCtrl.GetSimStatus() == "Ended"):
	return True
    else:
	return False

def isSimRunning():
    if (scriptGlobals.MainFrame.SimCtrl.GetSimStatus() == "Running"):
	return True
    else:
	return False

def simLoad(simFile): 
    scriptGlobals.MainFrame.SimCtrl.SelectScenario(simFile)
    scriptGlobals.MainFrame.Delay(1)
    logStr = "###### Loading SIM file: %s ######" % (simFile)
    scriptUtilities.logApp("*", logStr)
    
def simRun():   
    scriptGlobals.MainFrame.SimCtrl.RewindScenario()
    scriptGlobals.MainFrame.SimCtrl.RunScenario()
    scriptUtilities.logApp("*", "###### SIM starts ######")

def simStop():    
    scriptGlobals.MainFrame.SimCtrl.EndScenario(False)
    scriptGlobals.MainFrame.Delay(1)
    scriptUtilities.logApp("*", "###### SIM stops ######")
    
def simRewind():
    scriptUtilities.logApp("*", "###### SIM rewind ######")
    scriptGlobals.MainFrame.SimCtrl.RewindScenario()

def simSetAllChanAtten(atten):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",atten,1,True,True,True)
    logStr = "###### Set SIM atten: %.1f ######" % (atten)
    scriptUtilities.logApp("*", logStr)

def simOn():    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", 1,True,True,True)
    scriptUtilities.logApp("*", "###### Turn on simulator power ######")
    
def simOff():    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", 1,False,True,True)
    scriptUtilities.logApp("*", "###### Turn off simulator power ######")

def simOnChan(chan):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", chan,True,True,False)
    logStr = "###### Turn on channel %d ######" % (chan)    
    scriptUtilities.logApp("*", logStr)

def simOffChan(chan):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", chan,False,True,False)
    logStr = "###### Turn off channel %d ######" % (chan)    
    scriptUtilities.logApp("*", logStr)

def simOnSV(svID):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", svID,True,False,False)
    logStr = "###### Turn on SV %d ######" % (svID)    
    scriptUtilities.logApp("*", logStr)
    
def simOffSV(svID):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", svID,False,False,False)
    logStr = "###### Turn off SV %d ######" % (svID)    
    scriptUtilities.logApp("*", logStr)
    
def simSetChanAtten(atten, chan):    
    scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",atten,chan,True,False,True) 
    logStr = "###### Set SIM atten: %.1f on channel: %d ######" % (atten, chan)
    scriptUtilities.logApp("*", logStr)
    
def simSecondsIntoRun():    
    secIntoRun = scriptGlobals.MainFrame.SimCtrl.GetTimeIntoRun()
    logStr = "###### Second into run: %d ######" % (secIntoRun)
    return secIntoRun