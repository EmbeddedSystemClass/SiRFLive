import sys
import clr
import Site
import System
import System.Diagnostics
import nt
import os
import shutil
import ConfigParser
import time
import array 
from System import Array
from System import Threading

clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *

clr.AddReference('IronPython')
from IronPython.Runtime.Exceptions import PythonKeyboardInterruptException

clr.AddReferenceByPartialName('CommonClassLibrary')
from CommonClassLibrary import *

clr.AddReferenceByPartialName('SiRFLive')
from SiRFLive import *

import scriptGlobals

def areAllEqual(* args):
    myList = args[0]
    
    myRemovedList = list(set(myList))
    if (len(myRemovedList) == 1):
	return True
    else:
	return False
     
def format2Digits(number):
    # print number
    return "%02d" % (number)

def clearall():
    all = [var for var in globals() if var[0] != "_"]
    for var in all:
	del globals()[var]

def abortTest(* args):
 
    if (scriptGlobals.MainFrame.GetLoopitStatus() == True):
    	scriptGlobals.MainFrame.StopLoopit()
    if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	scriptGlobals.MainFrame.SimCtrl.EndScenario(False)  
    
    scriptGlobals.TestAborted = True 
    scriptGlobals.Exiting = True
    scriptGlobals.MainFrame.CancelDelay()
    comIdx = 0    
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (myPort):
	    if (myPort.comm.RxCtrl):
		if (myPort.comm.RxCtrl.ResetCtrl):
		    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
		    myPort.comm.RxCtrl.ResetCtrl.ResetTimerStop(False)		
		    myPort.comm.RxCtrl.ResetCtrl.CloseTTFFLog()
		myPort.comm.RxCtrl.LogCleanup()
	    myPort.comm.Log.CloseFile()
	    if (myPort.comm.ListenersCtrl):
		myPort.comm.ListenersCtrl.Stop()
		myPort.comm.ListenersCtrl.Cleanup()    
	    myPort.comm.ClosePort()	
	comIdx = comIdx + 1
	
    scriptGlobals.MainFrame.UpdateGUIFromScript()
    scriptGlobals.MainFrame.SetScriptDone(True)    
    scriptGlobals.Console.PythonEngineOutput.CloseFile()   

    scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr -= abortTest
    scriptGlobals.DutPortManagersList.Clear()
    scriptGlobals.DutMainPortsList.Clear()
    scriptGlobals.MainFrame.ClearPortList()
	
    # sys.exit(0)
    # mainT = Threading.Thread.CurrentThread
    # mainT.Abort(PythonKeyboardInterruptException(""))

def scriptClosePorts():
    comIdx = 0
    
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue	
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (myPort):
	    if (myPort.comm.ListenersCtrl):
		myPort.comm.ListenersCtrl.Stop()
		myPort.comm.ListenersCtrl.Cleanup()
	    myPort.comm.ClosePort()
	comIdx = comIdx + 1
    wT = 10
    logStr = "Wait %d to close ports ..." %(wT)
    logApp("*",logStr)
    scriptGlobals.MainFrame.Delay(wT)
    logApp("*","Done closing ports")
    
def openPorts():
   # setup each active com ports    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# reset logfile name
	myPort.comm.Log.SetDurationLoggingStatusLabel("")
	if (myPort.comm.IsSourceDeviceOpen() == False):
	    if (myPort.comm.OpenPort() == False):
		scriptGlobals.DutMainPortsList[comIdx] = -1
		comIdx = comIdx + 1 
		continue
		
	myPort.RunAsyncProcess()
	if (myPort.SignalViewLocation.IsOpen == False):
	    scriptGlobals.MainFrame.CreateSignalViewWin(myPort)
	if (myPort.DebugViewLocation.IsOpen == False):
	    scriptGlobals.MainFrame.CreateDebugViewWin(myPort)
	myPort.comm.RxCtrl.PollSWVersion()		

	# intialize flags to indicate RX get TTFF
	scriptGlobals.getTTFFs.append(False)
	scriptGlobals.getNavs.append(False)
	scriptGlobals.getTTBNavs.append(False)
	if (scriptGlobals.DutFamilyList[comIdx] == "GSD4e"):
	    if (scriptGlobals.DutResetPortsList[comIdx] != -1):
		myPort.comm.Init4eMPMWakeupPort(scriptGlobals.DutResetPortsList[comIdx])
		myPort.comm.Toggle4eWakeupPort()
		
	comIdx = comIdx + 1 
    
    scriptGlobals.MainFrame.UpdateGUIFromScript()
    
def logApp(comPort, msg):
    print msg
    alterPort = comPort.Replace(" " ,"")
    if(alterPort == "*"):
	for usePort in scriptGlobals.DutMainPortsList:
	    if (usePort < 0):		
		continue
	    myComName = `usePort`
	    myCom = scriptGlobals.MainFrame.GetPortRefByPortNum(myComName)
	    if (myCom):
		myCom.comm.WriteApp(msg)
    else:
	myComName = alterPort
	myCom = scriptGlobals.MainFrame.GetPortRefByPortNum(myComName)
	if (myCom):
	    myCom.comm.WriteApp(msg) 
   
def rfOnOff(onOff, onAtten):

    try:
	if (onOff == True):
	    attenStr = "###### Turn RF On #######"
	    logApp("*", attenStr)
	    # set atten 
	    if (scriptGlobals.AttenSource == General.clsGlobal.TESTRACK):
		scriptGlobals.MainFrame.TestRackCtrl.SetAtten(-1,int(onAtten))
	    elif (scriptGlobals.AttenSource == General.clsGlobal.SPAZ):
		scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(int(onAtten))
	    elif (scriptGlobals.AttenSource == General.clsGlobal.SIM):
		scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", 1,True,True,True)
	    else:
		# do nothing
		tmp = atten
	    
	else:
	    attenStr = "###### Turn RF Off #######"
	    logApp("*", attenStr)	
	    # set atten 
	    if (scriptGlobals.AttenSource == General.clsGlobal.TESTRACK):
		scriptGlobals.MainFrame.TestRackCtrl.SetAtten(-1,127)
	    elif (scriptGlobals.AttenSource == General.clsGlobal.SPAZ):
		scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(127)
	    elif (scriptGlobals.AttenSource == General.clsGlobal.SIM):
		scriptGlobals.MainFrame.SimCtrl.SetPowerOnOff("-", 1,False,True,True)
	    else:
		# do nothing
		tmp = atten
    except:
	print "Error turning RF On OFF"
	
def setAtten(atten):
    
    try:
	attenStr = "###### Setting Attenuation to " + `atten` + "dB #######"
	logApp("*", attenStr)
	# set atten 
	if ((scriptGlobals.AttenSource == General.clsGlobal.TESTRACK) or (scriptGlobals.AttenSource == General.clsGlobal.SPAZ)):
	    if (atten < 0) or (atten > 127):
		errorStr = "Atten = %.2f out of range 0-127" % (atten)
		logApp("*", errorStr)
		if (atten < 0):
		    atten = 0
		else: 
		    atten = 127
	    if (scriptGlobals.AttenSource == General.clsGlobal.TESTRACK):
		scriptGlobals.MainFrame.TestRackCtrl.SetAtten(-1,int(atten))
	    elif (scriptGlobals.AttenSource == General.clsGlobal.SPAZ):
		scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(int(atten))
	    else:
		# do nothing
		tmp = atten
	elif (scriptGlobals.AttenSource == General.clsGlobal.SIM):
	    scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",-atten,1,True,True,True)
	else:
	    # do nothing
	    tmp = atten
    except:
	print "Error setting atten %.2f" % (atten)
	
def SPAZOnOff(* args):
    if (len(args) > 0):
	onOff = args[0]
	if (scriptGlobals.PowerSource == General.clsGlobal.SPAZ):
	    
	    if (onOff == True):		
		displayStr = "Power ON Time %s" % (time.strftime("%H:%M:%S %Y/%m/%d ", time.localtime()))		
	    else:
		displayStr = "Power OFF Time %s" % (time.strftime("%H:%M:%S %Y/%m/%d ", time.localtime()))
	    logApp("*", displayStr)
	    scriptGlobals.MainFrame.SpazCtrl.WriteSPAzPower(onOff)
	else:
	    logApp("*", "Power Source not configured to use SPAz!")
    
def init(* args):
    if (len(args) > 0):
	resetType = args[0]
	reset(resetType)
    else:
	sendFactoryReset()
    logApp("*", "DUT Initializing...")
    scriptGlobals.MainFrame.Delay(2)
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.SetMessageRate(30, 0, 1, General.clsGlobal.SSB);
	myPort.comm.RxCtrl.SetMessageRate(28, 0, 1, General.clsGlobal.SSB);
	myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = True
	myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = True
	
	comIdx = comIdx + 1    

def reset(resetType):    
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.ResetCtrl.IsAidingPerformedOnFactory = False
	myPort.comm.RxCtrl.ResetCtrl.Reset(resetType)

	comIdx = comIdx + 1
    #if (resetType.Contains(General.clsGlobal.FACTORY)):
	#scriptGlobals.MainFrame.Delay(5)
	#switchToOSP()
	
def sendFactoryReset(* args):
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.ResetCtrl.IsAidingPerformedOnFactory = False
	myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.FACTORY)
	comIdx = comIdx + 1
	
    # scriptGlobals.MainFrame.Delay(5)
    # switchToOSP()

def switchToOSP(* args):
    
    comIdx = 0
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	if (scriptGlobals.DutFamilyList[comIdx] == "GSD4e"):
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    # myPort.comm.ClosePort()
	    myPort.comm.AutoDetectProtocolAndBaudDone = False;
	    myPort.comm.RxCurrentTransmissionType = Communication.CommunicationManager.TransmissionType.Text
	    myPort.comm.TxCurrentTransmissionType = Communication.CommunicationManager.TransmissionType.Text
	    myPort.comm.MessageProtocol = General.clsGlobal.NMEA
	    myPort.comm.BaudRate = "4800"    
	    # myPort.comm.SetupRxCtrl()
	    myPort.comm.comPort.BaudRate = 4800	    
	    myPort.comm.comPort.UpdateBaudSettings(4800);	    	    
	comIdx = comIdx + 1
    
    scriptGlobals.MainFrame.Delay(1)
    
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (scriptGlobals.DutFamilyList[comIdx] == "GSD4e"):
	    myPort.comm.WriteData("$PSRF105,1*3E")
	    switchStr = "$PSRF100,0," + scriptGlobals.DutBaudRateList[comIdx] + ",8,1,0"
	    switchStrWithChkSum = MessageHandling.NMEAReceiver.NMEA_AddCheckSum(switchStr)
	    # scriptGlobals.MainFrame.Delay(1)
	    toSendString = "$PSRF105,1*3E\r\n" + switchStrWithChkSum
	    myPort.comm.WriteData(toSendString)
	    
	    print "%s Detecting baud..." % (myPort.comm.PortName)	       
	comIdx = comIdx + 1   
    scriptGlobals.MainFrame.Delay(5)
    comIdx = 0    
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (scriptGlobals.DutFamilyList[comIdx] == "GSD4e"):
	    myPort.comm.RxCurrentTransmissionType = Communication.CommunicationManager.TransmissionType.GPS
	    myPort.comm.TxCurrentTransmissionType = Communication.CommunicationManager.TransmissionType.GP2	    
	    myPort.comm.RxType = Communication.CommunicationManager.ReceiverType.SLC
	    myPort.comm.MessageProtocol = General.clsGlobal.OSP
    
	    myPort.comm.BaudRate = scriptGlobals.DutBaudRateList[comIdx]
	    myPort.comm.comPort.BaudRate = int(scriptGlobals.DutBaudRateList[comIdx])
	    myPort.comm.comPort.UpdateBaudSettings(myPort.comm.comPort.BaudRate)	    
	    # myPort.comm.SetupRxCtrl()	
	    myPort.comm.RxCtrl.SetMessageRateForFactory();
	    myPort.comm.AutoDetectProtocolAndBaudDone = True
	    
	comIdx = comIdx + 1
    
    scriptGlobals.MainFrame.UpdateGUIFromScript()
    scriptGlobals.MainFrame.Delay(2)
def updateResetType():
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myStartmode = myPort.comm.SiRFNavStartStop.StartMode
	if (myStartmode == 0):
	    myPort.comm.RxCtrl.ResetCtrl.ResetType =  "Auto"
	elif (myStartmode == 1):
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.HOT
	elif (myStartmode == 2):
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.WARM_NO_INIT
	elif (myStartmode == 3):
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.COLD
	elif (myStartmode == 4):
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.FACTORY
	else:
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.HOT
	comIdx = comIdx + 1

def updateSiRFNavStartParams():
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# Config SiRFNavStart
	if (len(scriptGlobals.SiRFNavStartStop_Version) > comIdx):
	    myPort.comm.SiRFNavStartStop.Version = scriptGlobals.SiRFNavStartStop_Version[comIdx]
	else:
	    myPort.comm.SiRFNavStartStop.Version = scriptGlobals.SiRFNavStartStop_Version[0]
	myPort.comm.SiRFNavStartStop.StartMode = scriptGlobals.SiRFNavStartStop_StartMode
	myPort.comm.SiRFNavStartStop.UARTMaxPreamble = scriptGlobals.SiRFNavStartStop_UARTMaxPreamble
	myPort.comm.SiRFNavStartStop.UARTIdleByteWakeupDelay = scriptGlobals.SiRFNavStartStop_UARTIdleByteWakeupDelay
	myPort.comm.SiRFNavStartStop.ReferenceClockOffset = int(scriptGlobals.DutDefaultClksList[comIdx])#0x177FA
	myPort.comm.SiRFNavStartStop.FlowControl = scriptGlobals.SiRFNavStartStop_FlowControl
	myPort.comm.SiRFNavStartStop.LNAType = int(scriptGlobals.DutLNATypesList[comIdx])
	myPort.comm.SiRFNavStartStop.DebugSettings = scriptGlobals.SiRFNavStartStop_DebugSettings
	myPort.comm.SiRFNavStartStop.ReferenceClockWarmupDelay = scriptGlobals.SiRFNavStartStop_ReferenceClockWarmupDelay
	# myPort.comm.SiRFNavStartStop.ReferenceClockFrequency =  0xf9c568 # 0x18CBA80 
	myPort.comm.SiRFNavStartStop.ReferenceClockFrequency =  int(scriptGlobals.DutExtClkFreqsList[comIdx]) 
	myPort.comm.SiRFNavStartStop.ReferenceClockUncertainty = scriptGlobals.SiRFNavStartStop_ReferenceClockUncertainty
	myPort.comm.SiRFNavStartStop.BaudRate = scriptGlobals.SiRFNavStartStop_BaudRate
	myPort.comm.SiRFNavStartStop.IOPinConfigurationMode = scriptGlobals.SiRFNavStartStop_IOPinConfigurationMode
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[0] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration0
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[1] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration1
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[2] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration2
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[3] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration3
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[4] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration4
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[5] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration5
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[6] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration6
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[7] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration7
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[8] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration8
	myPort.comm.SiRFNavStartStop.IOPinConfiguration[9] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration9
	# myPort.comm.SiRFNavStartStop.IOPinConfiguration[10] = SiRFNavStartStop_IOPinConfiguration10
	if (myPort.comm.SiRFNavStartStop.Version > 1):
	    myPort.comm.SiRFNavStartStop.IOPinConfiguration[10] = scriptGlobals.SiRFNavStartStop_IOPinConfiguration10
	myPort.comm.SiRFNavStartStop.I2CHostAddress = scriptGlobals.SiRFNavStartStop_I2CHostAddress
	myPort.comm.SiRFNavStartStop.I2CTrackerAddress = scriptGlobals.SiRFNavStartStop_I2CTrackerAddress
	myPort.comm.SiRFNavStartStop.I2CMode = scriptGlobals.SiRFNavStartStop_I2CMode
	myPort.comm.SiRFNavStartStop.I2CRate = scriptGlobals.SiRFNavStartStop_I2CRate
	myPort.comm.SiRFNavStartStop.SPIRate = scriptGlobals.SiRFNavStartStop_SPIRate
	myPort.comm.SiRFNavStartStop.ONOffControl = scriptGlobals.SiRFNavStartStop_ONOffControl
	myPort.comm.SiRFNavStartStop.FlashMode = scriptGlobals.SiRFNavStartStop_FlashMode
	myPort.comm.SiRFNavStartStop.StorageMode = scriptGlobals.SiRFNavStartStop_StorageMode
	# for Phytec put "C"
	if (scriptGlobals.DutSiRFNavInterfaceStringsList[comIdx] == "-1"):
		trackerPortStr = "\\\\.\\COM" + `scriptGlobals.DutTrackerPortsList[comIdx]`
	else:
		trackerPortStr = scriptGlobals.DutSiRFNavInterfaceStringsList[comIdx]
	myPort.comm.SiRFNavStartStop.TrackerPort = trackerPortStr 
	myPort.comm.SiRFNavStartStop.TrackerPortSeleted = scriptGlobals.SiRFNavStartStop_TrackerPortSeleted
	myPort.comm.SiRFNavStartStop.WeakSignalEnable = scriptGlobals.SiRFNavStartStop_WeakSignalEnable
	myPort.comm.SiRFNavStartStop.LDOEnable = int(scriptGlobals.DutLDOModesList[comIdx])	
	
	comIdx = comIdx + 1
	
	
def waitForNav():
    
    comIdx = 0	
    isNav = True
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	scriptGlobals.getNavs[comIdx] = myPort.comm.m_NavData.IsNav
	isNav = isNav and scriptGlobals.getNavs[comIdx]
	print "Port %d wait for fix, nav flag = %d" % (usePort, scriptGlobals.getNavs[comIdx])
	comIdx = comIdx + 1      
	
    return isNav

def waitForTTBNav():
    
    comIdx = 0	
    isNav = True
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	scriptGlobals.getTTBNavs[comIdx] = myPort.comm.IsTTBNav()
	isNav = isNav and scriptGlobals.getTTBNavs[comIdx]
	print "Port %d TTB wait for fix, nav flag = %d" % (usePort, scriptGlobals.getNavs[comIdx])
	comIdx = comIdx + 1      
	
    return isNav


def checkTTFF():
    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	scriptGlobals.getTTFFs[comIdx] = myPort.comm.m_NavData.Valid
	 
	print "Port %d wait for fix, valid flag = %d" % (usePort, scriptGlobals.getTTFFs[comIdx])
	if (scriptGlobals.getTTFFs[comIdx] == False):
	    return False
	comIdx = comIdx + 1
    return True

def setResetInitParams():
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.UpdateResetInitParams()	
	 
	comIdx = comIdx + 1

def updateSingleDUTInfo(dutIdx):    
    myPort = scriptGlobals.DutPortManagersList[dutIdx]
    if (myPort):
	myPort.comm.UpdateTestSetup()
	myPort.comm.RxCtrl.DutStationSetup.TestRun = scriptGlobals.TestRun
	myPort.comm.RxCtrl.DutStationSetup.TestOperator = scriptGlobals.TestOperator
	myPort.comm.RxCtrl.DutStationSetup.StartTime = scriptGlobals.StartTime
	myPort.comm.RxCtrl.DutStationSetup.TestID = scriptGlobals.TestID
	myPort.comm.RxCtrl.DutStationSetup.TestName = scriptGlobals.TestName
	myPort.comm.RxCtrl.DutStationSetup.TestDescription = scriptGlobals.TestDescription
	myPort.comm.RxCtrl.DutStationSetup.DataClass = scriptGlobals.DataClass
	myPort.comm.RxCtrl.DutStationSetup.TestGroup = scriptGlobals.TestGroup
	myPort.comm.RxCtrl.DutStationSetup.RxSN = scriptGlobals.DutNamesList[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXAntType = scriptGlobals.SignalSource
	myPort.comm.RxCtrl.DutStationSetup.SignalType = scriptGlobals.SignalType
	myPort.comm.RxCtrl.DutStationSetup.SignalMargin = scriptGlobals.SignalMargin
	myPort.comm.RxCtrl.DutStationSetup.TTFFLimit = scriptGlobals.TTFFLimit
	myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = scriptGlobals.HrzErrorLimit
	myPort.comm.RxCtrl.DutStationSetup.TTFFTimeout = scriptGlobals.TTFFTimeout
	myPort.comm.RxCtrl.DutStationSetup.RXPlatformType = scriptGlobals.DUTPlatform[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXPackageType = scriptGlobals.DUTPackage[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXRev = scriptGlobals.DUTRev[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXVoltage = scriptGlobals.DUTVoltage[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXRefFreq = int(scriptGlobals.DutExtClkFreqsList[dutIdx])
	myPort.comm.RxCtrl.DutStationSetup.RXRefFreqSrc = scriptGlobals.DUTRefFreqSrc[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXPwrMode = scriptGlobals.DutLDOModesList[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXLNAType = scriptGlobals.DutLNATypesList[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXManufacture = scriptGlobals.DUTMfg[dutIdx]
	myPort.comm.RxCtrl.DutStationSetup.RXTempVal = ConvertToDouble(scriptGlobals.DUTTemperature[dutIdx])
	myPort.comm.RxCtrl.DutStationSetup.RXTempUnit = scriptGlobals.DUTTemperatureUnit[dutIdx]
   
    
def updateDUTInfo(dutIdx):
    if (dutIdx < 0):
	comIdx = 0 
	for usePort in scriptGlobals.DutMainPortsList:
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue
	    logStr = "Port %d: Update DUT Info..." %(usePort)
	    logApp(`usePort`, logStr)
	    updateSingleDUTInfo(comIdx)
	    comIdx = comIdx + 1	
	    
    else:
	usePort = scriptGlobals.DutMainPortsList[dutIdx]
	logStr = "Port %d: Update DUT Info..." %(usePort)
	logApp(`usePort`, logStr)
	updateSingleDUTInfo(dutIdx)	
	    
	    
def readNSaveTestConfig(* args):
   
    scriptGlobals.MainFrame.SetScriptDone(False)
    # scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr += abortTest
    General.clsGlobal.Abort = False
    General.clsGlobal.AbortSingle = False
    scriptGlobals.Exiting  = False  
    scriptGlobals.TestAborted = False
    
    scriptFullName = os.path.basename(scriptGlobals.MainFrame.TestScriptPath)
    print scriptFullName
    scriptAbsPath = os.path.dirname(scriptGlobals.MainFrame.TestScriptPath)
    scriptGlobals.ScriptName = scriptFullName .split(".")[0]    
    scriptGlobals.ScriptConfigFilePath = scriptAbsPath + "\\" + scriptGlobals.ScriptName + ".cfg";
    
    print "Running: %s" % (scriptGlobals.ScriptName)
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()    
    sCfg.read(scriptGlobals.ScriptConfigFilePath)
    
    testLevelStr = sCfg.get('TEST_PARAM','SIGNAL_LEVEL')
    scriptGlobals.SignalType = sCfg.get('TEST_PARAM','SIGNAL_TYPE')
    scriptGlobals.CableLoss = sCfg.getfloat('TEST_PARAM','CABLE_LOSS') 
    scriptGlobals.SimFile = sCfg.get('SIM','SIM_FILE')
    scriptGlobals.UseTTBForAiding = sCfg.getint('TTB_TIME_AIDING','ENABLE') 
    
    scriptGlobals.TestSignalLevelsList = Utilities.HelperFunctions.ParseTestLevels(testLevelStr)  

    if (scriptGlobals.AttenSource == General.clsGlobal.TESTRACK):
	scriptGlobals.MainFrame.SetTestRackInterface()	
    
    scriptGlobals.TestName = sCfg.get('TEST_PARAM','TEST_NAME')
    scriptGlobals.TestDescription = sCfg.get('TEST_PARAM','TEST_DESCRIPTION')
    scriptGlobals.DataClass = sCfg.get('TEST_PARAM','TEST_DATA_CLASS')
    scriptGlobals.TestGroup = sCfg.get('TEST_PARAM','TEST_GROUP')
    scriptGlobals.SignalMargin = ConvertToDouble(sCfg.get('TEST_PARAM','MARGIN'))
    scriptGlobals.TTFFLimit = ConvertToDouble(sCfg.get('TEST_PARAM','TTFF_LIMIT'))
    scriptGlobals.HrzErrorLimit = ConvertToDouble(sCfg.get('TEST_PARAM','2D_ERROR_LIMIT'))
    scriptGlobals.TTFFTimeout= General.clsGlobal.LoopitTimeout
    
def ConvertToDouble(inString):
    
    if (inString == "N/A"):
	result = -9999
    else:
	try:
	    result = float(inString)
	except:
	    result = -9999
    return result

def ExceptionHandler():    
    scriptError = sys.exc_info()
    if (General.clsGlobal.Abort == False):
	if (len(scriptError) >= 3):
	    trackBack = scriptError[2]
	    scriptErrorMessage = scriptError[1]
	    lineno = trackBack.tb_lineno
	    print "Error Message: %s -- Line: %d" % (scriptErrorMessage,lineno)
	print "Error running script: %s" %(scriptGlobals.ScriptName)
    
    sys.exc_clear()
    scriptGlobals.MainFrame.SetScriptDone(True) 
