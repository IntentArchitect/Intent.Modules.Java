package com.samples.app.application.services.impl;

import lombok.AllArgsConstructor;

import org.modelmapper.ModelMapper;
import org.modelmapper.TypeToken;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import com.samples.app.application.services.TestService;
import com.samples.app.domain.models.User;
import com.samples.app.intent.IntentMerge;
import com.samples.app.repository.UserRepository;
import com.samples.app.intent.IntentIgnoreBody;
import com.samples.app.application.models.UserUpdateDto;
import com.samples.app.application.models.UserDto;
import com.samples.app.application.models.UserCreateDto;

@Service
@AllArgsConstructor
@IntentMerge
public class TestServiceImpl implements TestService {

    private UserRepository userRepository;
    private ModelMapper mapper;

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public UserDto Get(long id) {
        var x = this.userRepository.findById(id).get();
        return new UserDto(x.getId(), x.getFirstName(), x.getLastName(), x.getFullName(), x.getPhoneNumber(), x.getEmail(), x.getUsername(), x.getPassword());
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Create(UserCreateDto dto) {
        var entity = new User(
            dto.getFirstName(), 
            dto.getLastName(), 
            dto.getFullName(), 
            dto.getUsername(), 
            dto.getPhoneNumber(), 
            dto.getEmail(),
            dto.getPassword());
        this.userRepository.save(entity);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Update(long id, UserUpdateDto dto) {
        var x = this.userRepository.findById(id).get();
        x.setFirstName(dto.getFirstName());
        x.setLastName(dto.getLastName());
        x.setFullName(dto.getFullName());
        x.setPhoneNumber(x.getPhoneNumber());
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Delete(long id) {
        var x = this.userRepository.findById(id).get();
        this.userRepository.delete(x);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public List<UserDto> FindAll() {
        var users = this.userRepository.findAll();
        return this.mapper.map(users, new TypeToken<List<UserDto>>() {}.getType());
    }
}