clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *
import time
import random

import scriptGlobals
import scriptUtilities  
import scriptSim

def checkTTFF():
    global getTTFFs
    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	getTTFFs[comIdx] = myPort.comm.m_NavData.Valid
	 
	print "Port %d waits for fix, valid flag = %d" % (usePort, getTTFFs[comIdx])
	if (getTTFFs[comIdx] == False):
	    return False
	comIdx = comIdx + 1
    return True



try:    
    General.clsGlobal.Abort = False
    General.clsGlobal.AbortSingle = False
    scriptGlobals.Exiting = False
    scriptGlobals.TestAborted = False
    
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()    
    sCfg.read(scriptGlobals.ScriptConfigFilePath)
        
    # timeout = 26
    General.clsGlobal.LoopitTimeout = int(scriptUtilities.ConvertToDouble(sCfg.get('TEST_PARAM','TIMEOUT')))
    General.clsGlobal.LoopitIteration = sCfg.getint('TEST_PARAM','NUMBER_OF_LOOP') 
    General.clsGlobal.LoopitResetType = sCfg.get('TEST_PARAM','RESET_TYPE')
    
    earlyCompletionFlag = sCfg.getint('TEST_PARAM','ALLOW_EARLY_TERMINATION')
    if (earlyCompletionFlag == 1):
	General.clsGlobal.LoopitEarlyTermination = True
    else:
	General.clsGlobal.LoopitEarlyTermination = False  
	
    if (sCfg.getint('TEST_PARAM','DELAY_POSITION') == 1):
	delayPositionAiding = True
    else:
	delayPositionAiding = False
    if (sCfg.getint('TEST_PARAM','DELAY_TIME') == 1):
	delayTimeAiding = True
    else:
	delayAidingTime = False
    if (sCfg.getint('TEST_PARAM','DELAY_FREQ') == 1):
	delayTimeAiding = True
    else:
	delayFreqAiding = False
    delayAidingTime = sCfg.getint('TEST_PARAM','DELAY_TIME_SEC')

    #scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr += scriptUtilities.abortTest    
    
    if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	scriptGlobals.MainFrame.SimCtrl.EndScenario(False)
	scriptGlobals.MainFrame.SimCtrl.SelectScenario(scriptGlobals.SimFile)
	scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",scriptGlobals.SimInitialAtten,1,True,True,True)
	scriptGlobals.MainFrame.SimCtrl.RunScenario()    
    
    # set to high level for factory reset
    if (scriptGlobals.SignalType.ToLower() == "dbhz"):
	defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,40,scriptGlobals.SignalType)
    elif (scriptGlobals.SignalType.ToLower() == "dbm"):
	defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,-130,scriptGlobals.SignalType)
    else:
	print "Signal Type is not correct"
	defaultAtten = 0
    if (scriptGlobals.AttenSource == General.clsGlobal.SIM):
	defaultAtten = -scriptGlobals.SimInitialAtten;
    # set atten 
    scriptUtilities.setAtten(defaultAtten)    
    
    testPowerStr = ""
    for testPower in scriptGlobals.TestSignalLevelsList:
	testPowerStr = testPowerStr + " " + `testPower`
    print "Test power levels: %s" % (testPowerStr)

    getTTFFs = []
    getPosAck = []
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
	# winComList[comIdx].comm.RxCtrl.OpenChannel("SSB")
	myPort.comm.RxCtrl.PollSWVersion()
	myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)	
	myPort.comm.RxName = scriptGlobals.DutNamesList[comIdx]
	myDefaultClkIdx = scriptGlobals.DutDefaultClkIndicesList[comIdx]
	myPort.comm.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex = myDefaultClkIdx
	myPort.comm.AutoReplyCtrl.FreqTransferCtrl.FreqOffset = General.clsGlobal.DEFAULT_RF_FREQ[myDefaultClkIdx]

	# open TTB port and send aiding if TTB aiding is enable
	myTTBPort = scriptGlobals.DutTTBPortsList[comIdx]
	if (( myTTBPort > 0) and (scriptGlobals.UseTTBForAiding == 1)):
	    
	    print "Port %d connect TTB port %d" % (usePort, myTTBPort)
	    myPort.comm.OpenTTBPort(myTTBPort)
	    myPort.comm.SendTTBAiding()	
        	
	if (myPort.SignalViewLocation.IsOpen == False):
		scriptGlobals.MainFrame.CreateSignalViewWin(myPort)
	if (myPort.DebugViewLocation.IsOpen == False):
		scriptGlobals.MainFrame.CreateDebugViewWin(myPort)
	if (myPort.TTFFDisplayLocation.IsOpen == False):
		scriptGlobals.MainFrame.CreateTTFFWin(myPort)
	
	comIdx = comIdx + 1
	getTTFFs.append(False)
	getPosAck.append(False)
    
    scriptUtilities.sendFactoryReset()    
    scriptGlobals.MainFrame.Delay(10)
    # setup each active com ports    
    comIdx = 0	
    for usePort in portList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.OpenChannel("SSB")
	myPort.comm.RxCtrl.OpenChannel("STAT")
	# winComList[comIdx].comm.RxCtrl.SendRaw("A0A20033E400010101010101010101010101010101010501010101010101010101010101010101010101010101010101010101010101010119B0B3")
	comId = comIdx + 1
    navStatus = False
    count = 0
    while((navStatus == False) and (count < 12)):
	navStatus = scriptGlobals.MainFrame.GetNavStatus("*")
	count = count + 1
	print "Wait for nav loop %d" %(count)
	if ((navStatus == True) or (count >= 12)): 		
	    break
	# if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
	    # testAborted = True
	    # break
	mainFrame.Delay(10)
    
    # if (testAborted == True):
	# print "Test aborted"
	# sys.exit(0)
	
    if(navStatus == True):
	print "RX(s) navigate -- start test now"
    else:
	print "Rx(s) not navigate"	    
    
    ttbWaitTime = 900
    msgString = "Wait %d seconds for TTB to navigate?" % (ttbWaitTime)
    if (scriptGlobals.UseTTBForAiding == 1):
	# last element is timeout in ms
	result = MessageBoxEx.Show(msgString, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000);

	if (result == DialogResult.Yes):
	    # ttbWaitTime = 2
	    displayStr = "%s: Wait %d sec for TTB to navigate and collect aiding information%s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),ttbWaitTime, "\r\n\r\n    ")
	    scriptUtilities.logApp("*",displayStr)
	    result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,ttbWaitTime*1000);	    
	else:
	    print "Skip TTB waiting";

    for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
	level = scriptGlobals.TestSignalLevelsList[levelIndex]
	if (scriptGlobals.TestAborted == True):
	    print "Test aborted"
	    break
	scriptUtilities.setAtten(defaultAtten)
	nbWaitTime = 900
	msgString = "Wait %d seconds to collect GPS information?" % (nbWaitTime)
	if (scriptGlobals.UseTTBForAiding == 0):
	    # last element is timeout in ms
	    result = MessageBoxEx.Show(msgString, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000);

	    if (result == DialogResult.Yes):
		if (scriptGlobals.MainSignalSource == General.clsGlobal.SIM):
		    scriptGlobals.MainFrame.SimCtrl.EndScenario(False)
		    scriptGlobals.MainFrame.SimCtrl.SelectScenario(simFile)
		    scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",mainSimInitAtten,1,True,True,True)
		    scriptGlobals.MainFrame.SimCtrl.RunScenario()
		
		    scriptUtilities.sendFactoryReset()
		
		# ttbWaitTime = 2
		displayStr = "%s: Wait %d sec for RX to navigate and collectGPS information %s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),nbWaitTime, "\r\n\r\n    ")
		scriptUtilities.logApp("*",displayStr)
		result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,nbWaitTime*1000);
	
	
	atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
	diffAtten = atten - defaultAtten
	if (diffAtten > 5):
	    dropAtten = divmod(atten,5)
	    drop5dBLoop = dropAtten[0]
	    restAtten = dropAtten[1]
	    
	    for dropIndex in range(0, int(drop5dBLoop)+1):		
		atten1 = 5*dropIndex + defaultAtten
		scriptUtilities.setAtten(atten1)		
		scriptGlobals.MainFrame.Delay(20)
	    
	    atten1 = restAtten +  atten1
	    scriptUtilities.setAtten(atten1)
	    
	else:
	    scriptUtilities.setAtten(atten)
	
	print "Testing level: %d" % (level)
	
	# setup each active com ports    
	comIdx = 0	
	for usePort in scriptGlobals.DutMainPortsList:
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue 
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    myPort.comm.RxCtrl.PollSWVersion()
	    myPort.comm.RxCtrl.OpenChannel("SSB")
	    myPort.comm.RxCtrl.OpenChannel("STAT")	
	    # Create directory for log files
	    Now = time.localtime(time.time())
	    timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
	    # portLogFile = scriptGlobals.TestResultsDirectory + timeNowStr + "_" + scriptGlobals.DutNamesList[comIdx] + "_com" + `usePort` + "_" + scriptGlobals.ScriptName + "_" + `level` + scriptGlobals.LogFileExtsList[comIdx]
	    # ttffLogFile = scriptGlobals.TestResultsDirectory + timeNowStr + "_" + scriptGlobals.DutNamesList[comIdx] + "_com" + `usePort` + "_" + scriptGlobals.ScriptName + "_" + `level` + "_ttff.csv"
	    baseName = "%s%s_%s_%s_%s_%s" %(scriptGlobals.TestResultsDirectory,timeNowStr,scriptGlobals.DutNamesList[comIdx],myPort.comm.PortName,scriptGlobals.ScriptName,level)
	    portLogFile = baseName + scriptGlobals.LogFileExtsList[comIdx]
	    ttffLogFile = baseName + "_ttff.csv"
	    myPort.comm.RxCtrl.ResetCtrl.ResetInterval = General.clsGlobal.LoopitTimeout
	    myPort.comm.RxCtrl.ResetCtrl.TotalNumberOfResets = General.clsGlobal.LoopitIteration
	    myPort.comm.RxCtrl.ResetCtrl.ResetEarlyTerminate = General.clsGlobal.LoopitEarlyTermination
	    myPort.comm.RxCtrl.ResetCtrl.ResetType = General.clsGlobal.LoopitResetType
	    
	    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = True
	    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = True
	    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableFullSystemReset = True
	    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False	
	    myPort.comm.RxCtrl.SetValidatePostionFlag(True)	
	    myPort.comm.RxCtrl.PollSWVersion()
	    # update Test Info	    
	    if (level < 0):
		testSetupLevelString = "%s%s%s" %("m",`-level`.replace('.','p'),scriptGlobals.SignalType) 	
	    else:
		testSetupLevelString = "%s%s" %(`level`.replace('.','p'),scriptGlobals.SignalType) 
		
	    scriptGlobals.TestID = "%s-%s" % (scriptGlobals.TestName,testSetupLevelString)	 
	    Now = time.localtime(time.time())
	    scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
	    myPort.comm.RxCtrl.DutStationSetup.SignalLevel = level
	    scriptUtilities.updateDUTInfo(comIdx)
	    myPort.comm.m_TestSetup.Atten = atten
	    
	    #open log file
	    myPort.comm.Log.OpenFile(portLogFile)
	    myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)	    
	    
	    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
	    # myPort.comm.RxCtrl.ResetCtrl.ResetTimerStart(True)	    
	    
	    comIdx = comIdx + 1	
	testSamples = General.clsGlobal.LoopitIteration
	scriptGlobals.MainFrame.UpdateGUIFromScript()
	for cycle in range(0,testSamples):
	    if (scriptGlobals.TestAborted == True):
		print "Test aborted"
		break
	    testCycleString = "###### Reset Number %d ######" % (cycle+1) 
	    scriptUtilities.logApp("*",testCycleString)
	
	    # setup each active com ports    
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue 
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		if (delayPositionAiding == True):
		    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Reject = True
		    myPort.comm.AutoReplyCtrl.PushAidingMask = myPort.comm.AutoReplyCtrl.PushAidingMask or 0x1
		    myPort.comm.AutoReplyCtrl.ForceAidingRequestMask = myPort.comm.AutoReplyCtrl.ForceAidingRequestMask or 0x1
		if (delayTimeAiding == True):
		    myPort.comm.AutoReplyCtrl.TimeTransferCtrl.Reject = True
		    myPort.comm.AutoReplyCtrl.PushAidingMask = myPort.comm.AutoReplyCtrl.PushAidingMask or 0x4
		    myPort.comm.AutoReplyCtrl.ForceAidingRequestMask = myPort.comm.AutoReplyCtrl.ForceAidingRequestMask or 0x4
		if (delayFreqAiding == True):
		    myPort.comm.AutoReplyCtrl.FreqTransferCtrl.Reject = True
		    myPort.comm.AutoReplyCtrl.PushAidingMask = myPort.comm.AutoReplyCtrl.PushAidingMask or 0x2
		    myPort.comm.AutoReplyCtrl.ForceAidingRequestMask = myPort.comm.AutoReplyCtrl.ForceAidingRequestMask or 0x2
		
		myPort.comm.AutoReplyCtrl.PushAidingAvailability = True
		myPort.comm.AutoReplyCtrl.PushAidingDelay = delayAidingTime
		
		myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.LoopitResetType)
		comIdx = comIdx + 1	    
	    
	    waitTTFFLoops = 0
	    WAIT_TTFF_TIMEOUT = General.clsGlobal.LoopitTimeout/5	    
	    
	    # setup each active com ports    
	    while(waitTTFFLoops < WAIT_TTFF_TIMEOUT):
		comIdx = 0	
		for usePort in scriptGlobals.DutMainPortsList:
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue 
		    posAckInt = 0
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    getPosAck[comIdx] = myPort.comm.AutoReplyCtrl.PosReqAck
		    if (getPosAck[comIdx] == True):
			myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Reject = False		
			myPort.comm.AutoReplyCtrl.TimeTransferCtrl.Reject = False		
			myPort.comm.AutoReplyCtrl.FreqTransferCtrl.Reject = False
					    
		    print "%s: position ack flag:%s" % (myPort.comm.PortName, getPosAck[comIdx] )
		    
		    comIdx = comIdx + 1
		getAllPosAck = True
		for gi in range(0,len(getPosAck)):
		    getAllPosAck = getAllPosAck and getPosAck[gi]
		
		if (getAllPosAck == True):
		    break
		else:
		    mainFrame.Delay(2)
		    waitTTFFLoops = waitTTFFLoops + 1		
	    waitTTFFLoops = 0
	    while(waitTTFFLoops < WAIT_TTFF_TIMEOUT):
		if(checkTTFF() == True):
		    break
		else:
		    mainFrame.Delay(5)
		waitTTFFLoops = waitTTFFLoops + 1
	    comIdx = 0
	    for usePort in scriptGlobals.DutMainPortsList:
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		if (waitTTFFLoops >= WAIT_TTFF_TIMEOUT):
		    # log result
		    scriptGlobals.DutPortManagersList[comIdx].comm.RxCtrl.ResetCtrl.LogTTFFCsv()		
		comIdx = comIdx + 1	    
	    
	    # add 10 seconds so log close correctly
	    randStartTime = 10
	    
	    logString = "Sleep %ds" % (randStartTime)
	    scriptUtilities.logApp("*",logString)
	    mainFrame.Delay(randStartTime)   
	
	#cleanup for this run
	scriptGlobals.Exiting = True
	comIdx = 0
	for usePort in scriptGlobals.DutMainPortsList:
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue	
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    myPort.comm.Log.CloseFile();
	    myPort.comm.RxCtrl.ResetCtrl.CloseTTFFLog();
	    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
	    comIdx = comIdx + 1
	    
    #cleanup for all levels
    scriptGlobals.Exiting = True
    scriptUtilities.scriptClosePorts()

    # set default atten
    scriptUtilities.setAtten(defaultAtten)
        
    timeoutSpec = General.clsGlobal.LoopitTimeout - 1;
    ttffLimitStr = `timeoutSpec` + "," + `timeoutSpec` + "," + `timeoutSpec`

    myReport = mainFrame.ReportCtrl
    myReport.Percentile = "50,95"
    myReport.TTFFLimit = ttffLimitStr
    myReport.HrErrLimit = "50,150"
    myReport.TimeoutVal = `timeoutSpec`
    myReport.LimitVal = "100"
    myReport.TTFFReportType = 3
    #if (isAidingTest != 0):
	#myReport.TTFFReportType = 3
    #else:
	#myReport.TTFFReportType = 2
    myReport.ReportLayout = SiRFLive.Reporting.Report.ReportLayoutType.GroupBySWVersion
    myReport.Summary_Reset_V2(logDirectory)
    
    myReport.ReportLayout = SiRFLive.Reporting.Report.ReportLayoutType.GroupByResetType
    myReport.Summary_Reset_V2(logDirectory)
    
    print "Done: %s" % (scriptGlobals.ScriptName)
    
except:
    scriptUtilities.ExceptionHandler()    
