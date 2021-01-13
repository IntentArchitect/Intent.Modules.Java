package com.samples.app.application.mappings;

import org.modelmapper.PropertyMap;
import com.samples.app.domain.models.User;
import com.samples.app.application.models.UserDto;

public class UserToUserDtoMapping extends PropertyMap<User, UserDto> {
    protected void configure() {
        map().setPhone(source.getPhoneNumber());
        map().setEmailAddress(source.getEmail());
    }
}