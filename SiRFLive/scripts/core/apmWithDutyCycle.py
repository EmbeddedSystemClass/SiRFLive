import scriptGlobals
import scriptUtilities
import scriptSim

def arraySizeCheck(* args):
    
    if (len(APMNumFixesArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Num Fixes", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMDutyCycleArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Duty Cycle", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMMaxHrzErrorArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %ss" % ("Max Horz Error", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMMaxVrtErrorArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Max Vert Error", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMPriorityArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Priority", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMMaxOffTimeArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Max Off Time", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMMaxSearchTimeArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Max Search Time", "Time Between Fix")
	errorCount = errorCount + 1
    if (len(APMTimeAccPriorityArray) != len(APMTBF)):
	print "Error: Length Not Equal %s VS %s" % ("Max Time Acc Priority", "Time Between Fix")
	errorCount = errorCount + 1
    if (errorCount == 0):
	return True
    else:
	return False

try:  
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()
    sCfg.read(scriptGlobals.ScriptConfigFilePath )
    
    logTime = sCfg.getint('TEST_PARAM', 'LOG_TIME')
    APMNumFixesArray = sCfg.get('POWER_PARAM', 'NUM_FIXES')
    APMTBFArray = sCfg.get('POWER_PARAM', 'TIME_BETWEEN_FIX')
    APMDutyCycleArray = sCfg.get('POWER_PARAM', 'DUTY_CYCLE')
    APMMaxHrzErrorArray = sCfg.get('POWER_PARAM', 'MAX_HRZ_ERROR')
    APMMaxVrtErrorArray = sCfg.get('POWER_PARAM', 'MAX_VRT_ERROR')
    APMPriorityArray = sCfg.get('POWER_PARAM', 'PRIORITY')
    APMMaxOffTimeArray = sCfg.get('POWER_PARAM', 'MAX_OFF_TIME')
    APMMaxSearchTimeArray = sCfg.get('POWER_PARAM', 'MAX_SEARCH_TIME')
    APMTimeAccPriorityArray = sCfg.get('POWER_PARAM', 'TIME_ACC_PRIORITY')
    
    # Convert string to array
    APMNumFixesArray = eval('['+APMNumFixesArray+']')
    APMTBFArray = eval('['+APMTBFArray+']') 
    APMDutyCycleArray = eval('['+APMDutyCycleArray+']') 
    APMMaxHrzErrorArray = eval('['+APMMaxHrzErrorArray+']') 
    APMMaxVrtErrorArray = eval('['+APMMaxVrtErrorArray+']') 
    APMPriorityArray = eval('['+APMPriorityArray+']') 
    APMMaxOffTimeArray = eval('['+APMMaxOffTimeArray+']') 
    APMMaxSearchTimeArray = eval('['+APMMaxSearchTimeArray+']') 
    APMTimeAccPriorityArray = eval('['+APMTimeAccPriorityArray+']')    
   
    testContinue = True;
    errorCount = 0
    if (arraySizeCheck == False): 
	scriptGlobals.TestAborted = True
    else:	
	if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	    if (scriptSim.isSimRunning() == True):
		result = MessageBoxEx.Show("SIM is running -- Proceed?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000)
		if (result == DialogResult.Yes):
		    testContinue = True
		    scriptSim.simStop()
		    scriptSim.simLoad(scriptGlobals.SimFile)		
		    scriptSim.simRun()
		else:
		    testContinue = False
	    else:
		scriptSim.simLoad(scriptGlobals.SimFile)	    
		scriptSim.simRun()
	if (testContinue == True):
	    scriptGlobals.MainFrame.Delay(5)
	    # set to high level for factory reset
	    if (scriptGlobals.SignalType.ToLower() == "dbhz"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,40,scriptGlobals.SignalType)
	    elif (scriptGlobals.SignalType.ToLower() == "dbm"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,-130,scriptGlobals.SignalType)
	    else:
		print "Signal Type is not correct"
		defaultAtten = 0
	    # set atten
	    scriptUtilities.setAtten(defaultAtten)    
	    
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
			portList[comIdx] = -1
			continue
		    myPort.RunAsyncProcess()
		if (myPort.SignalViewLocation.IsOpen == False):
		    mainFrame.CreateSignalViewWin(myPort)
		if (myPort.DebugViewLocation.IsOpen == False):
		    mainFrame.CreateDebugViewWin(myPort)
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.PollSWVersion()
		myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)
		myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
	
		comIdx = comIdx + 1
	    scriptUtilities.init()
	    mainFrame.UpdateGUIFromScript()
	    mainFrame.Delay(10)
	    # setup each active com ports
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.OpenChannel("STAT")
	
		comId = comIdx + 1
	    # Loop for APM configuration
	    for APMIndex in range(len(APMNumFixesArray)):
		if (scriptGlobals.TestAborted == True):
		    print "Test aborted"
		    break
		APMNumFixes = APMNumFixesArray[APMIndex]
		APMTBF = APMTBFArray[APMIndex]
		APMDutyCycle = APMDutyCycleArray[APMIndex]
		APMMaxHrzError = APMMaxHrzErrorArray[APMIndex]
		APMMaxVrtError = APMMaxVrtErrorArray[APMIndex]
		APMPriority = APMPriorityArray[APMIndex]
		APMMaxOffTime = APMMaxOffTimeArray[APMIndex]
		APMMaxSearchTime = APMMaxSearchTimeArray[APMIndex]
		APMTimeAccPriority = APMTimeAccPriorityArray[APMIndex]
		
		if (APMNumFixes != 0):
		    APMNumFixes = 0
		    scriptUtilities.logApp("*", "Invalid number of fixes -- Set to 0")

		# Loop for each test levels    
		for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
		    level = scriptGlobals.TestSignalLevelsList[levelIndex]
		    if (scriptGlobals.TestAborted == True):
			print "Test aborted"
			break
		    atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
		    diffAtten = atten - defaultAtten
		    if ((levelIndex == 0) and (diffAtten > 5) and (atten > 5)):
			dropAtten = divmod(atten,5)
			drop5dBLoop = dropAtten[0]
			restAtten = dropAtten[1]
	    
			for dropIndex in range(0, int(drop5dBLoop)+1):
			    atten1 = 5*dropIndex + defaultAtten
			    scriptUtilities.setAtten(atten1)
			    mainFrame.Delay(20)
	    
			atten1 = restAtten +  atten1
			scriptUtilities.setAtten(atten1)
	    
		    else:
			scriptUtilities.setAtten(atten)		    

		    # setup each active com ports
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.RxCtrl.OpenChannel("SSB")
			myPort.comm.RxCtrl.OpenChannel("STAT")
			myPort.comm.RxCtrl.PollSWVersion()
			# Create directory for log files
			Now = time.localtime(time.time())
			timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
			testName = scriptGlobals.ScriptName
			portLogFile = "%s%s_%d-%d_%s_%s_%s%s" %(scriptGlobals.TestResultsDirectory,testName,APMTBF,APMDutyCycle,scriptGlobals.DutNamesList[comIdx],timeNowStr,myPort.comm.PortName,scriptGlobals.LogFileExtsList[comIdx])			
			    
			scriptGlobals.TestID = "%s-%d-%d" % (scriptGlobals.TestName,APMTBF,APMDutyCycle)	 
			Now = time.localtime(time.time())
			scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)

			scriptUtilities.updateDUTInfo(comIdx)
			myPort.comm.m_TestSetup.Atten = atten
			
			myPort.comm.Log.OpenFile(portLogFile)
	    
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableFullSystemReset = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False
	    
			myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
	    
			# Send full power mode
			myPort.comm.LowPowerParams.Mode = 0;
			myPort.comm.RxCtrl.SetPowerMode(False);
	    
			comIdx = comIdx + 1

		    navStatus = False
		    count = 0
		    while((navStatus == False) and (count < 12)):
			navStatus = mainFrame.GetNavStatus("*")
			count = count + 1
			print "Wait for nav loop %d" %(count)
			if ((navStatus == True) or (count >= 12)):
			    break
			# if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
			    # scriptGlobals.TestAborted = True
			    # break
			mainFrame.Delay(5)
	    
		    # Set APM mode
		    print "APM(TBF=%d,DC=%d,HzErr=%d,VrErr=%d) begins..." %(APMTBF,APMDutyCycle,APMMaxHrzError,APMMaxVrtError)
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.LowPowerParams.Mode = 1
			myPort.comm.LowPowerParams.APMNumFixes = APMNumFixes
			myPort.comm.LowPowerParams.APMTBF = APMTBF
			myPort.comm.LowPowerParams.APMDutyCycle = APMDutyCycle
			myPort.comm.LowPowerParams.APMMaxHrzError = APMMaxHrzError
			myPort.comm.LowPowerParams.APMMaxVrtError = APMMaxVrtError
			myPort.comm.LowPowerParams.APMPriority = APMPriority
			myPort.comm.LowPowerParams.APMMaxOffTime = APMMaxOffTime
			myPort.comm.LowPowerParams.APMMaxSearchTime = APMMaxSearchTime
			myPort.comm.LowPowerParams.APMTimeAccPriority = APMTimeAccPriority			
	    
			myPort.comm.RxCtrl.SetPowerMode(False);
			comIdx = comIdx + 1
	    
		    logStr = "%s: Start logging for %d seconds ... " % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),logTime)
		    scriptUtilities.logApp("*", logStr)
		    mainFrame.Delay(logTime)
	    
		    #cleanup
	
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			# Send full power mode
			myPort.comm.LowPowerParams.Mode = 0;
			myPort.comm.RxCtrl.SetPowerMode(False);
			myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
			myPort.comm.Log.CloseFile()		    
			comIdx = comIdx + 1  
		
		# wait for cycle to end
		print "%s: Wait for cycle end (%d seconds) ... " % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),APMTBF)
		mainFrame.Delay(APMTBF)
		# End Loop for APM Configuration
		
	    # set default atten
	    scriptUtilities.setAtten(defaultAtten)	    
	    print "Done: %s" % (scriptGlobals.ScriptName)
    
    mainFrame.SetScriptDone(True)
	
    #cleanup
    scriptGlobals.Exiting  = True
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# Send full power mode
	myPort.comm.ListenersCtrl.Stop()
	myPort.comm.ListenersCtrl.Cleanup()
	myPort.comm.ClosePort()
	# myPort.StopAsyncProcess()
	comIdx = comIdx + 1

except:
    scriptUtilities.ExceptionHandler()

