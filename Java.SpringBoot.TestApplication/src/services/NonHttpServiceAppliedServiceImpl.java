package root.src.services;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import root.src.contracts.NonHttpServiceAppliedService;
import root.src.intent.IntentIgnoreBody;
import root.src.intent.IntentMerge;

@Service
@AllArgsConstructor
@IntentMerge
public class NonHttpServiceAppliedServiceImpl implements NonHttpServiceAppliedService {

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Operation1() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}