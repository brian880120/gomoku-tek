import React from 'react';
import PropTypes from 'prop-types';

const SelectInput = ({name, onChange, defaultOption, value, options}) => {
    return (
        <select name={name}
            value={value}
            onChange={onChange}
            className="ui compact selection dropdown">
            <option value={defaultOption.value}>{defaultOption.text}</option>
            {options.map((option) => {
                return <option key={option.value} value={option.value}>{option.text}</option>;
            })}
        </select>
    );
};

SelectInput.propTypes = {
    name: PropTypes.string,
    onChange: PropTypes.func,
    defaultOption: PropTypes.object,
    value: PropTypes.string,
    options: PropTypes.arrayOf(PropTypes.object)
};

export default SelectInput;
