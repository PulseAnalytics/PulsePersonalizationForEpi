(function () {
    return {
        uiCreated: function (namingContainer, settings) {
            var control = dijit.byId(namingContainer + 'SegmentId');
            
            var updateUI = function () {
                var descriptionNode = dojo.byId(namingContainer + "description");
                var segmentId = dijit.byId(namingContainer + 'SegmentId').get("value");
             
                if (segmentId != "") {
                    dojo.xhrGet({
                        url: '../../../MarketSegments/GetSegmentDiscription?segmentName=' + segmentId,
                        handleAs: 'json',
                        preventCache: true,
                        error: epi.cms.ErrorDialog.showXmlHttpError,
                        load: function (discription) {
                            console.log(discription)
                            descriptionNode.innerHTML = "<div style='width:600px'><p>"
                                + "<a href='https://browse.pulse.app/?c_id=" + segmentId + "' target='_blank'>"
                                + "<img src = 'https://pulsestatic.z13.web.core.windows.net/" + segmentId + ".png' style='margin-top:-40px;padding: 20px 20px; vertical-align:middle;float:right;' /> "
                                + "</a>"
                                + discription
                                + "</p><p><a href='https://browse.pulse.app/?c_id=" + segmentId + "&traffic=hidden' target='_blank' style='color:blue;'>View the Map</a></p>"
                                + "</p></div>";
                        }
                    });               
                }
            }

            updateUI();

            control.on('change', function (evt) {
                updateUI();
            });
        }
    };
})();