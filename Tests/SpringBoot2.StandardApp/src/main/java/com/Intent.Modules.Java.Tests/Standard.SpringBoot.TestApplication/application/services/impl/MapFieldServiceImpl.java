package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.MapFieldService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import java.util.Map;
import org.springframework.transaction.annotation.Transactional;

@Service
@AllArgsConstructor
@IntentMerge
public class MapFieldServiceImpl implements MapFieldService {
    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String ReceiveMapParam(Map<String, String> map) {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}
