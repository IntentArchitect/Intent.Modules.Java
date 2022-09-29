package root.src.services;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import root.src.contracts.HttpServiceAppliedService;
import root.src.intent.IntentIgnoreBody;
import root.src.intent.IntentMerge;

@Service
@AllArgsConstructor
@IntentMerge
public class HttpServiceAppliedServiceImpl implements HttpServiceAppliedService {

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetValue() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostValue(String value) {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void NonAppliedOperation() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}