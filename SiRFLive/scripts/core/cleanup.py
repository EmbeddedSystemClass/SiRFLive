try:    
    mainFrame.UpdateGUIFromScript()
    for hostOpen in hostAppDirs:
	    mainFrame.HostAppCtrl.CloseWin(hostOpen)
except:
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
	    