var ajaxFileUploadParams={};
jQuery.extend({
	initAjaxUpload:function(params){
	ajaxFileUploadParams={};
	ajaxFileUploadParams=params; 
	},
	getUploadParams:function(){
		return ajaxFileUploadParams;
	},
    createUploadIframe: function(id, uri)
	{
			//create frame
            var frameId = 'jUploadFrame' + id;
            var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
			if(window.ActiveXObject)
			{
                if(typeof uri== 'boolean'){
					iframeHtml += ' src="' + 'javascript:false' + '"';

                }
                else if(typeof uri== 'string'){
					iframeHtml += ' src="' + uri + '"';

                }	
			}
			iframeHtml += ' />';
			jQuery(iframeHtml).appendTo(document.body);

            return jQuery('#' + frameId).get(0);			
    },
    createUploadForm: function(id, fileElementId, data)
	{
		//create form	
		var formId = 'jUploadForm' + id;
		var fileId = 'jUploadFile' + id;
		var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');	
		if(data)
		{
			for(var i in data)
			{
				jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
			}			
		}		
		var oldElement = jQuery('#' + fileElementId);
		var newElement = jQuery(oldElement).clone();
		jQuery(oldElement).attr('id', fileId);
		jQuery(oldElement).before(newElement);
		jQuery(oldElement).appendTo(form);
		//set attributes
		jQuery(form).css('position', 'absolute');
		jQuery(form).css('top', '-1200px');
		jQuery(form).css('left', '-1200px');
		jQuery(form).appendTo('body');		
		return form;
    },

    ajaxFileUpload: function(s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime()        
		var form = jQuery.createUploadForm(id, s.fileElementId, (typeof(s.data)=='undefined'?false:s.data));
		var io = jQuery.createUploadIframe(id, s.secureuri);
		var frameId = 'jUploadFrame' + id;
		var formId = 'jUploadForm' + id;		
        // Watch for a new set of requests
       // if ( s.global && ! jQuery.active++ )
		//{
		//	jQuery.event.trigger( "ajaxStart" );
		//}
		if(s.start){
			var start=(eval(s.start));
			start();
		}
		$("#ajax_upload_main").fadeOut();
		$("#ajax_loading_div").css("top",windowHeight()/2-30+$(document).scrollTop());
		$("#ajax_loading_div").css("left",windowWidth()/2-30);
		$("#ajax_loading_div").css("display","block");
		
		
        var requestDone = false;
        // Create the request object
        var xml = {}   
        if ( s.global )
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function(isTimeout)
		{			
			var io = document.getElementById(frameId);
            try 
			{				
				if(io.contentWindow)
				{
					 xml.responseText = io.contentWindow.document.body?io.contentWindow.document.body.innerHTML:null;
                	 xml.responseXML = io.contentWindow.document.XMLDocument?io.contentWindow.document.XMLDocument:io.contentWindow.document;
					 
				}else if(io.contentDocument)
				{
					 xml.responseText = io.contentDocument.document.body?io.contentDocument.document.body.innerHTML:null;
                	xml.responseXML = io.contentDocument.document.XMLDocument?io.contentDocument.document.XMLDocument:io.contentDocument.document;
				}						
            }catch(e)
			{
				jQuery.handleError(s, xml, null, e);
			}
            if ( xml || isTimeout == "timeout") 
			{				
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if ( status != "error" )
					{
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData( xml, s.dataType );    
                        // If a local callback was specified, fire it and pass it the data
                        if ( s.success )
                            s.success( data, status );
    
                        // Fire the global callback
                        if( s.global )
                            jQuery.event.trigger( "ajaxSuccess", [xml, s] );
                    } else
                        jQuery.handleError(s, xml, status);
                } catch(e) 
				{
                    status = "error";
                    jQuery.handleError(s, xml, status, e);
                }

                // The request was completed
               // if( s.global )
                //    jQuery.event.trigger( "ajaxComplete", [xml, s] );
                if(s.finish){
                	var finish=(eval(s.finish));
					finish(xml,s);
                }
				$("#upload_background_DIV").css("display","none");
				$("#ajax_loading_div").css("display","none");
                // Handle the global AJAX counter
                if ( s.global && ! --jQuery.active )
                    jQuery.event.trigger( "ajaxStop" );

                // Process result
                if ( s.complete )
                    s.complete(xml, status);

                jQuery(io).unbind()

                setTimeout(function()
									{	try 
										{
											jQuery(io).remove();
											jQuery(form).remove();	

										} catch(e) 
										{
											jQuery.handleError(s, xml, null, e);
										}									

									}, 100)

                xml = null

            }
        }
        // Timeout checker
        if ( s.timeout > 0 ) 
		{
            setTimeout(function(){
                // Check to see if the request is still happening
                if( !requestDone ) uploadCallback( "timeout" );
            }, s.timeout);
        }
        try 
		{

			var form = jQuery('#' + formId);
			jQuery(form).attr('action', s.url);
			jQuery(form).attr('method', 'POST');
			jQuery(form).attr('target', frameId);
            if(form.encoding)
			{
				jQuery(form).attr('encoding', 'multipart/form-data');      			
            }
            else
			{	
				jQuery(form).attr('enctype', 'multipart/form-data');			
            }			
            jQuery(form).submit();

        } catch(e) 
		{			
            jQuery.handleError(s, xml, null, e);
        }
		
		jQuery('#' + frameId).load(uploadCallback	);
        return {abort: function () {}};	

    },

    uploadHttpData: function( r, type ) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if ( type == "script" )
            jQuery.globalEval( data );
        // Get the JavaScript object, if JSON is used.
        if ( type == "json" )
            eval( "data = " + data );
        // evaluate scripts within html
        if ( type == "html" )
            jQuery("<div>").html(data).evalScripts();

        return data;
    }
});
 
 	function ajax_upload_open()
	{
		$("#upload_background_DIV").css("display","block");
	 	$("#ajax_upload_main").css("top",windowHeight()/2-70+$(document).scrollTop());
		$("#ajax_upload_main").css("left",windowWidth()/2-150);
		$("#ajax_upload_main").fadeIn(); 
	}

	function ajax_upload_close(){
		$("#ajax_upload_main").fadeOut();
		$("#upload_background_DIV").css("display","none");
	}
	function ajaxFileUpload()
	{ 
		var obj=$.getUploadParams();
		if(obj.file_types){
			var file_names=$("#fileToUpload").val().split("\\");
			var file_name=file_names[file_names.length-1];//文件名
			var file_exts=file_name.split(".");
			var file_ext=file_exts[file_exts.length-1];//后缀
			var user_file_types="["+obj.file_types.replace(/\,/g,"][")+"]";
			if(user_file_types.toLowerCase().indexOf("["+file_ext.toLowerCase()+"]")!=-1||obj.file_types=="*"){
			}else{
				alert("此文件类型不允许上传,请上传"+user_file_types+"类型文件");
				return false;
			}
			
		}
		$.ajaxFileUpload
		(
			{
				url:obj.url,
				secureuri:false,
				fileElementId:'fileToUpload',
				dataType: obj.dataType?obj.dataType:"text",
				data:obj.data?obj.data:{},
				start:obj.start?obj.start:'',
				success:obj.success, 
				finish:obj.finish?obj.finish:'',
				error: obj.error?obj.error:''
			}
		)
	}
	
	//以下的方法对于很多浏览器都适用   

function windowHeight() {

	var windowHeight;//最后传出的值   
	if (self.innerHeight) { // 除了IE以外的浏览器   
		windowHeight = self.innerHeight;
	}
	else if (document.documentElement && document.documentElement.clientHeight) {
		windowHeight = document.documentElement.clientHeight;
	}
	else if (document.body) { //其他版本的IE浏览器   
		windowHeight = document.body.clientHeight;
	}
	return windowHeight;

}

function windowWidth() {

	var windowWidth;//最后传出的值   

	if (self.innerWidth) { // 除了IE以外的浏览器   
		windowWidth = self.innerWidth;
	}
	else if (document.documentElement && document.documentElement.clientWidth) {
		windowWidth = document.documentElement.clientWidth;
	}
	else if (document.body) { // 其他版本的IE浏览器   
		windowWidth = document.body.clientWidth;
	}
	return windowWidth;

}

